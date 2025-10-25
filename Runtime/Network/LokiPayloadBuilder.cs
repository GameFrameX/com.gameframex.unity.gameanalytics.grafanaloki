using System.Collections.Generic;
using GameFrameX.Runtime;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// Loki负载构建器
    /// </summary>
    public static class LokiPayloadBuilder
    {
        /// <summary>
        /// 构建Loki格式的JSON负载
        /// </summary>
        /// <param name="logs">日志条目列表</param>
        /// <param name="labels">标签字典</param>
        /// <returns>JSON字符串</returns>
        public static string BuildPayload(List<GameAnalyticsEntry> logs, Dictionary<string, string> labels)
        {
            if (logs == null || logs.Count == 0)
            {
                return string.Empty;
            }

            var grafanaLokiAnalyticsData = new GrafanaLokiAnalyticsData();
            var grafanaLokiAnalyticsDataStreamsItem = new GrafanaLokiAnalyticsDataStreamsItem();
            grafanaLokiAnalyticsData.streams.Add(grafanaLokiAnalyticsDataStreamsItem);

            // 添加标签
            foreach (var label in labels)
            {
                grafanaLokiAnalyticsDataStreamsItem.stream[label.Key] = label.Value;
            }

            // 添加日志条目
            foreach (var log in logs)
            {
                var values = new List<string> { log.TimestampNs.ToString(), log.GetEventDataJson(), };
                grafanaLokiAnalyticsDataStreamsItem.values.Add(values);
            }

            return Utility.Json.ToJson(grafanaLokiAnalyticsData);
        }
    }
}