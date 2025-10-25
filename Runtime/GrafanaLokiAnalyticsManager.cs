using System;
using System.Collections;
using System.Collections.Generic;
using GameFrameX.GameAnalytics.Runtime;
using GameFrameX.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// Grafana Loki分析管理器
    /// </summary>
    public class GrafanaLokiAnalyticsManager : BaseGameAnalyticsManager
    {
        private LogBatchManager _logBatchManager;
        private ILogStorage _logStorage;
        private LokiHttpClient _httpClient;
        private GrafanaLokiAnalyticsSetting _setting;
        private readonly Dictionary<string, object> _publicProperties = new Dictionary<string, object>();
        private Dictionary<string, string> _lokiLabels = new Dictionary<string, string>();
        private readonly Dictionary<string, long> _timerStartTimes = new Dictionary<string, long>();
        private readonly Dictionary<string, long> _timerPausedDurations = new Dictionary<string, long>();
        private bool _isManualInit = false;
        private string _playerId = "";
        private MonoBehaviour _coroutineRunner;
        private Coroutine _sendLogsCoroutine;
        private bool _isSending = false;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="args">参数</param>
        public override void Init(Dictionary<string, string> args)
        {
            if (m_IsInit)
            {
                return;
            }

            try
            {
                // 创建协程运行器
                var gameObject = new GameObject("[GrafanaLokiAnalyticsCoroutineRunner]");
                Object.DontDestroyOnLoad(gameObject);
                _coroutineRunner = gameObject.AddComponent<GrafanaLokiAnalyticsCoroutineRunner>();
                gameObject.hideFlags = HideFlags.HideInHierarchy;
                // 创建设置
                _setting = new GrafanaLokiAnalyticsSetting();

                // 应用参数
                if (args != null)
                {
                    if (args.TryGetValue("LokiUrl", out string lokiUrl))
                    {
                        _setting.LokiUrl = lokiUrl;
                    }

                    if (args.TryGetValue("BatchSendIntervalSeconds", out string intervalStr) && int.TryParse(intervalStr, out int interval))
                    {
                        _setting.BatchSendIntervalSeconds = interval;
                    }

                    if (args.TryGetValue("MaxBatchSize", out string maxBatchSizeStr) && int.TryParse(maxBatchSizeStr, out int maxBatchSize))
                    {
                        _setting.MaxBatchSize = maxBatchSize;
                    }
                }

                // 初始化批处理管理器
                _logBatchManager = new LogBatchManager(_setting.MaxBatchSize);

                // 初始化存储
                _logStorage = new SqliteLogStorage();

                // 初始化HTTP客户端
                _httpClient = new LokiHttpClient(_setting.LokiUrl, new Dictionary<string, string>
                {
                    { "Accept-Encoding", "gzip, deflate, br" },
                });

                // 初始化标签
                _lokiLabels = new Dictionary<string, string>();
                UpdateLabels();
                // 添加设备信息到公共属性
                AddDeviceInfoToPublicProperties();

                // 启动定时发送协程
                StartSendLogsCoroutine();
                m_IsInit = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"初始化GrafanaLokiAnalyticsManager失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 手动初始化
        /// </summary>
        /// <param name="args">参数</param>
        public override void ManualInit(Dictionary<string, string> args)
        {
            if (!m_IsInit)
            {
                return;
            }

            _isManualInit = true;
        }

        /// <summary>
        /// 是否手动初始化
        /// </summary>
        /// <returns>是否手动初始化</returns>
        public override bool IsManualInit()
        {
            return _isManualInit;
        }

        /// <summary>
        /// 设置玩家ID
        /// </summary>
        /// <param name="playerId">玩家ID</param>
        public override void SetPlayerId(string playerId)
        {
            _playerId = playerId;
            SetPublicProperties("player_id", playerId);
        }

        /// <summary>
        /// 设置公共属性
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public override void SetPublicProperties(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            _publicProperties[key] = value;
        }

        /// <summary>
        /// 清除公共属性
        /// </summary>
        public override void ClearPublicProperties()
        {
            _publicProperties.Clear();
            AddDeviceInfoToPublicProperties();
        }

        private void UpdateLabels()
        {
            // 设备基本信息
            _lokiLabels["device_id"] = SystemInfo.deviceUniqueIdentifier;
            _lokiLabels["device_model"] = SystemInfo.deviceModel;
            _lokiLabels["device_type"] = SystemInfo.deviceType.ToString();

            // 操作系统信息
            _lokiLabels["os"] = SystemInfo.operatingSystem;

            // 应用程序信息
            _lokiLabels["app_version"] = Application.version;
            _lokiLabels["unity_version"] = Application.unityVersion;
            _lokiLabels["platform"] = Application.platform.ToString();

            // 设备基本信息
            _publicProperties["device_id"] = SystemInfo.deviceUniqueIdentifier;
            _publicProperties["device_model"] = SystemInfo.deviceModel;
            _publicProperties["device_type"] = SystemInfo.deviceType.ToString();

            // 操作系统信息
            _publicProperties["os"] = SystemInfo.operatingSystem;

            // 应用程序信息
            _publicProperties["app_version"] = Application.version;
            _publicProperties["unity_version"] = Application.unityVersion;
            _publicProperties["platform"] = Application.platform.ToString();
        }

        /// <summary>
        /// 获取公共属性
        /// </summary>
        /// <returns>公共属性字典</returns>
        public override Dictionary<string, object> GetPublicProperties()
        {
            return new Dictionary<string, object>(_publicProperties);
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public override void StartTimer(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            _timerStartTimes[eventName] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _timerPausedDurations[eventName] = 0;
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public override void PauseTimer(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || !_timerStartTimes.ContainsKey(eventName))
            {
                return;
            }

            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long elapsed = now - _timerStartTimes[eventName];
            _timerPausedDurations[eventName] += elapsed;
            _timerStartTimes[eventName] = 0; // 标记为暂停状态
        }

        /// <summary>
        /// 恢复计时
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public override void ResumeTimer(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || !_timerPausedDurations.ContainsKey(eventName))
            {
                return;
            }

            _timerStartTimes[eventName] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 结束计时
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public override void StopTimer(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || !_timerStartTimes.ContainsKey(eventName))
            {
                return;
            }

            long duration = _timerPausedDurations[eventName];
            if (_timerStartTimes[eventName] > 0) // 如果不是暂停状态
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                duration += now - _timerStartTimes[eventName];
            }

            // 发送计时事件
            Dictionary<string, object> customFields = new Dictionary<string, object>
            {
                { "duration_ms", duration }
            };

            Event(eventName + "_duration", customFields);

            // 清理计时器数据
            _timerStartTimes.Remove(eventName);
            _timerPausedDurations.Remove(eventName);
        }

        /// <summary>
        /// 上报事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        public override void Event(string eventName)
        {
            Event(eventName, new Dictionary<string, object>());
        }

        /// <summary>
        /// 上报带有数值的事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventValue">事件数值</param>
        public override void Event(string eventName, float eventValue)
        {
            Dictionary<string, object> customFields = new Dictionary<string, object>
            {
                { "value", eventValue }
            };

            Event(eventName, customFields);
        }

        /// <summary>
        /// 上报自定义字段的事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="customFields">自定义字段</param>
        public override void Event(string eventName, Dictionary<string, object> customFields)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            try
            {
                // 创建日志条目
                GameAnalyticsEntry gameAnalyticsEntry = new GameAnalyticsEntry(eventName, customFields, _publicProperties);
                _logStorage.SaveLog(gameAnalyticsEntry);
            }
            catch (Exception ex)
            {
                Debug.LogError($"上报事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 上报带有数值和自定义字段的事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventValue">事件数值</param>
        /// <param name="customFields">自定义字段</param>
        public override void Event(string eventName, float eventValue, Dictionary<string, object> customFields)
        {
            if (customFields == null)
            {
                customFields = new Dictionary<string, object>();
            }

            customFields["value"] = eventValue;
            Event(eventName, customFields);
        }

        /// <summary>
        /// 添加设备信息到公共属性
        /// </summary>
        private void AddDeviceInfoToPublicProperties()
        {
            // 系统硬件信息
            _publicProperties["processor_type"] = SystemInfo.processorType;
            _publicProperties["processor_count"] = SystemInfo.processorCount;
            _publicProperties["processor_frequency"] = SystemInfo.processorFrequency;
            _publicProperties["system_memory_size"] = SystemInfo.systemMemorySize;

            // 图形相关信息
            _publicProperties["graphics_device_name"] = SystemInfo.graphicsDeviceName;
            _publicProperties["graphics_device_type"] = SystemInfo.graphicsDeviceType.ToString();
            _publicProperties["graphics_memory_size"] = SystemInfo.graphicsMemorySize;
            _publicProperties["graphics_device_version"] = SystemInfo.graphicsDeviceVersion;
            _publicProperties["graphics_shader_level"] = SystemInfo.graphicsShaderLevel;

            // 屏幕信息
            _publicProperties["screen_width"] = Screen.width;
            _publicProperties["screen_height"] = Screen.height;
            _publicProperties["screen_dpi"] = Screen.dpi;
            _publicProperties["screen_refresh_rate"] = Screen.currentResolution.refreshRate;
            // 添加本地化语言信息
            _publicProperties["system_language"] = Application.systemLanguage.ToString();
            _publicProperties["current_culture"] = System.Globalization.CultureInfo.CurrentCulture.Name;

            // 网络类型
            string networkType = Application.internetReachability switch
            {
                NetworkReachability.NotReachable                   => "No Network",
                NetworkReachability.ReachableViaCarrierDataNetwork => "Mobile Data",
                NetworkReachability.ReachableViaLocalAreaNetwork   => "WiFi",
                _                                                  => "Unknown"
            };
            _publicProperties["network_type"] = networkType;
        }

        /// <summary>
        /// 启动定时发送协程
        /// </summary>
        private void StartSendLogsCoroutine()
        {
            if (_coroutineRunner != null && _sendLogsCoroutine == null)
            {
                _sendLogsCoroutine = _coroutineRunner.StartCoroutine(SendLogsRoutine());
            }
        }

        /// <summary>
        /// 停止定时发送协程
        /// </summary>
        private void StopSendLogsCoroutine()
        {
            if (_coroutineRunner != null && _sendLogsCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_sendLogsCoroutine);
                _sendLogsCoroutine = null;
            }
        }

        /// <summary>
        /// 发送日志协程
        /// </summary>
        /// <returns>协程</returns>
        private IEnumerator SendLogsRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_setting.BatchSendIntervalSeconds);

                // 检查是否有网络连接
                if (LokiHttpClient.IsNetworkConnected() && !_isSending)
                {
                    _isSending = true;
                    _coroutineRunner.StartCoroutine(SendPendingLogs());
                }
            }
        }

        /// <summary>
        /// 发送待处理的日志
        /// </summary>
        /// <returns>协程</returns>
        private IEnumerator SendPendingLogs()
        {
            // 获取待发送的批次
            List<GameAnalyticsEntry> batch = null;

            try
            {
                // 添加到批处理队列
                var gameAnalyticsEntry = _logStorage.LoadPendingLogs(_setting.MaxBatchSize);
                foreach (var analyticsEntry in gameAnalyticsEntry)
                {
                    _logBatchManager.AddLogEntry(analyticsEntry);
                }

                batch = _logBatchManager.GetBatchForSending();
            }
            catch (Exception ex)
            {
                Debug.LogError($"获取日志批次时发生错误: {ex.Message}");
                _isSending = false;
                yield break;
            }

            if (batch == null || batch.Count <= 0)
            {
                _isSending = false;
                yield break;
            }

            // 构建负载
            string payload;
            try
            {
                payload = LokiPayloadBuilder.BuildPayload(batch, _lokiLabels);
            }
            catch (Exception ex)
            {
                Debug.LogError($"构建日志负载时发生错误: {ex.Message}");
                _isSending = false;
                yield break;
            }

            // 发送日志
            bool sendComplete = false;
            string errorMessage = "";

            yield return _httpClient.SendLogsCoroutine(payload, (success, error) =>
            {
                sendComplete = true;
                errorMessage = error;

                try
                {
                    if (success)
                    {
                        // 清除已处理的日志
                        var logIds = new List<string>(batch.Count);
                        foreach (var logEntry in batch)
                        {
                            logIds.Add(logEntry.Id);
                        }

                        _logStorage.ClearProcessedLogs(logIds);
                    }
                    else
                    {
                        Log.Warning($"发送日志失败: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"处理日志发送结果时发生错误: {ex.Message}");
                }
            });

            // 等待发送完成
            while (!sendComplete)
            {
                yield return null;
            }

            _isSending = false;
        }


        /// <summary>
        /// 关闭时的处理
        /// </summary>
        protected override void Shutdown()
        {
            // 停止定时发送协程
            StopSendLogsCoroutine();

            base.Shutdown();
        }
    }
}