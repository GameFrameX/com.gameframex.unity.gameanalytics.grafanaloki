using UnityEngine;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    public class GameFrameXGrafanaLokiAnalyticsCroppingHelper : MonoBehaviour
    {
        private void Start()
        {
            _ = typeof(GrafanaLokiAnalyticsSetting);
            _ = typeof(GrafanaLokiAnalyticsManager);
            _ = typeof(GrafanaLokiAnalyticsData);
            _ = typeof(GrafanaLokiAnalyticsDataStreamsItem);
            _ = typeof(GrafanaLokiAnalyticsCoroutineRunner);
            _ = typeof(LokiPayloadBuilder);
            _ = typeof(LokiHttpClient);
            _ = typeof(ILogStorage);
            _ = typeof(SqliteLogStorage);
            _ = typeof(LogBatchManager);
            _ = typeof(GameAnalyticsEntry);
        }
    }
}