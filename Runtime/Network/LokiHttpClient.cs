using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// Loki HTTP客户端
    /// </summary>
    public sealed class LokiHttpClient
    {
        private readonly string _lokiUrl;
        private readonly Dictionary<string, string> _headers;
        private readonly int _timeoutSeconds;
        private readonly int _maxRetries;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">Loki服务器URL</param>
        /// <param name="headers">HTTP头部</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <param name="maxRetries">最大重试次数</param>
        public LokiHttpClient(string url, Dictionary<string, string> headers = null, int timeoutSeconds = 10, int maxRetries = 3)
        {
            _lokiUrl = url;
            _headers = headers ?? new Dictionary<string, string>();
            _timeoutSeconds = timeoutSeconds;
            _maxRetries = maxRetries;

            // 确保Content-Type头部存在
            if (!_headers.ContainsKey("Content-Type"))
            {
                _headers["Content-Type"] = "application/json";
            }
        }

        private class LokiCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        /// <summary>
        /// 发送日志
        /// </summary>
        /// <param name="payload">JSON负载</param>
        /// <param name="callback">回调</param>
        /// <returns>协程</returns>
        public IEnumerator SendLogsCoroutine(string payload, Action<bool, string> callback)
        {
            if (string.IsNullOrEmpty(payload))
            {
                callback?.Invoke(false, "负载为空");
                yield break;
            }

            int retryCount = 0;
            bool success = false;
            string errorMessage = "";

            while (!success && retryCount < _maxRetries)
            {
                // 如果是重试，添加延迟（指数退避）
                if (retryCount > 0)
                {
                    float delay = Mathf.Pow(2, retryCount - 1);
                    yield return new WaitForSeconds(delay);
                }

                using (UnityWebRequest request = new UnityWebRequest(_lokiUrl, "POST"))
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
                    request.certificateHandler = new LokiCertificateHandler();
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.timeout = _timeoutSeconds;

                    // 添加头部
                    foreach (var header in _headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }

                    // 发送请求
                    yield return request.SendWebRequest();

                    // 检查结果
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        success = true;
                    }
                    else
                    {
                        errorMessage = $"HTTP错误: {request.error}";
                        retryCount++;
                    }
                }
            }

            callback?.Invoke(success, errorMessage);
        }

        /// <summary>
        /// 检查网络连接状态
        /// </summary>
        /// <returns>是否连接</returns>
        public static bool IsNetworkConnected()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}