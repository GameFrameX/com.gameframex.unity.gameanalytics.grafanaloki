using System.Collections.Generic;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// 日志批处理管理器
    /// </summary>
    public sealed class LogBatchManager
    {
        private readonly Queue<GameAnalyticsEntry> _logQueue = new Queue<GameAnalyticsEntry>();
        private readonly object _queueLock = new object();
        private readonly int _maxBatchSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxBatchSize">最大批量大小</param>
        public LogBatchManager(int maxBatchSize = 100)
        {
            _maxBatchSize = maxBatchSize;
        }

        /// <summary>
        /// 添加日志条目
        /// </summary>
        /// <param name="entry">日志条目</param>
        public void AddLogEntry(GameAnalyticsEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            lock (_queueLock)
            {
                _logQueue.Enqueue(entry);
            }
        }

        /// <summary>
        /// 获取待发送的批次
        /// </summary>
        /// <returns>日志条目列表</returns>
        public List<GameAnalyticsEntry> GetBatchForSending()
        {
            List<GameAnalyticsEntry> batch = new List<GameAnalyticsEntry>();

            lock (_queueLock)
            {
                int count = System.Math.Min(_logQueue.Count, _maxBatchSize);
                for (int i = 0; i < count; i++)
                {
                    if (_logQueue.Count > 0)
                    {
                        batch.Add(_logQueue.Dequeue());
                    }
                }
            }

            return batch;
        }

        /// <summary>
        /// 获取队列中的日志数量
        /// </summary>
        /// <returns>日志数量</returns>
        public int GetQueueCount()
        {
            lock (_queueLock)
            {
                return _logQueue.Count;
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void ClearQueue()
        {
            lock (_queueLock)
            {
                _logQueue.Clear();
            }
        }
    }
}