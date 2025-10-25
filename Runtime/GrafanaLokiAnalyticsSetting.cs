using System;
using GameFrameX.GameAnalytics.Runtime;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// Grafana Loki分析设置
    /// </summary>
    [Serializable]
    public class GrafanaLokiAnalyticsSetting : BaseGameAnalyticsSetting
    {
        /// <summary>
        /// Loki服务器URL
        /// </summary>
        public string LokiUrl = "http://localhost:3100/loki/api/v1/push";

        /// <summary>
        /// 批量发送间隔（秒）
        /// </summary>
        public int BatchSendIntervalSeconds = 5;

        /// <summary>
        /// 最大批量大小
        /// </summary>
        public int MaxBatchSize = 20;
    }
}