using System.Collections.Generic;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// 日志存储接口
    /// </summary>
    public interface ILogStorage
    {
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="gameAnalyticsEntry"></param>
        void SaveLog(GameAnalyticsEntry gameAnalyticsEntry);

        /// <summary>
        /// 加载待处理的日志
        /// </summary>
        /// <param name="maxBatchSize"></param>
        /// <returns>日志条目列表</returns>
        List<GameAnalyticsEntry> LoadPendingLogs(int maxBatchSize);

        /// <summary>
        /// 清除已处理的日志
        /// </summary>
        /// <param name="ids">已处理的日志条目Id列表</param>
        void ClearProcessedLogs(List<string> ids);

        /// <summary>
        /// 清除所有日志
        /// </summary>
        void ClearAllLogs();
    }
}