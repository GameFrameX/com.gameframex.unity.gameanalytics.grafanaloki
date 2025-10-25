using System;
using System.Collections.Generic;
using GameFrameX.Runtime;
using SQLite;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// 日志条目
    /// </summary>
    [Serializable]
    public sealed class GameAnalyticsEntry
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Indexed]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件数据
        /// </summary>
        public string EventDataJson { get; set; }

        /// <summary>
        /// 时间戳（纳秒）
        /// </summary>
        public long TimestampNs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="publicProperties">公共属性</param>
        public GameAnalyticsEntry(string eventName, Dictionary<string, object> eventData, Dictionary<string, object> publicProperties = null)
        {
            EventName = eventName;
            // 添加事件数据
            if (publicProperties == null)
            {
                publicProperties = new Dictionary<string, object>();
            }

            if (eventData == null)
            {
                eventData = new Dictionary<string, object>();
            }

            var result = publicProperties;

            foreach (var kvp in eventData)
            {
                result[kvp.Key] = kvp.Value;
            }

            // 设置事件时间
            result["event_time"] = DateTime.UtcNow;
            EventDataJson = Utility.Json.ToJson(result);

            CreateTime = DateTime.UtcNow;

            // 设置时间戳（转换为纳秒）
            TimestampNs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000000;

            // 生成唯一ID
            Id = Guid.NewGuid().ToString("N");
        }

        [Preserve]
        public GameAnalyticsEntry()
        {
        }

        /// <summary>
        /// 获取JSON格式的事件数据
        /// </summary>
        /// <returns>JSON字符串</returns>
        public string GetEventDataJson()
        {
            return Utility.Json.ToJson(new EventDataWrapper(EventName, EventDataJson));
        }

        /// <summary>
        /// 事件数据包装器（用于JSON序列化）
        /// </summary>
        [Serializable]
        public sealed class EventDataWrapper
        {
            public string event_name;
            public string event_data;

            public EventDataWrapper(string eventName, string eventData)
            {
                event_name = eventName;
                event_data = eventData;
            }
        }
    }
}