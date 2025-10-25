using System;
using System.Collections.Generic;
using System.IO;
using GameFrameX.Runtime;
using UnityEngine;
using SQLite;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Runtime
{
    /// <summary>
    /// SQLite日志存储
    /// </summary>
    public class SqliteLogStorage : ILogStorage
    {
        private readonly object _fileLock = new object();
        readonly SQLiteConnection _sqLiteConnection;
        readonly TableQuery<GameAnalyticsEntry> _tableQuery;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logFilePath">日志文件路径</param>
        public SqliteLogStorage(string logFilePath = "grafana_loki.db")
        {
            try
            {
                // 确保路径以应用程序持久化数据路径为基础
                var logDirectory = Path.Combine(Application.persistentDataPath, logFilePath);
                _sqLiteConnection = new SQLiteConnection(logDirectory);
                var tableInfo = _sqLiteConnection.GetTableInfo(nameof(GameAnalyticsEntry));
                if (tableInfo == null || tableInfo.Count == 0)
                {
                    _sqLiteConnection.CreateTable<GameAnalyticsEntry>();
                }

                _tableQuery = _sqLiteConnection.Table<GameAnalyticsEntry>();
            }
            catch (Exception e)
            {
                Log.Fatal(e);
            }
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="logs">日志条目列表</param>
        public void SaveLog(GameAnalyticsEntry logs)
        {
            lock (_fileLock)
            {
                try
                {
                    _sqLiteConnection.Insert(logs);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"保存日志失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 加载待处理的日志
        /// </summary>
        /// <param name="maxBatchSize"></param>
        /// <returns>日志条目列表</returns>
        public List<GameAnalyticsEntry> LoadPendingLogs(int maxBatchSize)
        {
            lock (_fileLock)
            {
                try
                {
                    return _tableQuery.OrderBy(m => m.CreateTime).Take(maxBatchSize).ToList();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"加载日志失败: {ex.Message}");
                }

                return new List<GameAnalyticsEntry>();
            }
        }


        /// <summary>
        /// 清除已处理的日志
        /// </summary>
        /// <param name="ids">已处理的日志条目Id列表</param>
        public void ClearProcessedLogs(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return;
            }

            lock (_fileLock)
            {
                try
                {
                    foreach (var logId in ids)
                    {
                        _sqLiteConnection.Delete<GameAnalyticsEntry>(logId);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"清除已处理日志失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 清除所有日志
        /// </summary>
        public void ClearAllLogs()
        {
            lock (_fileLock)
            {
                try
                {
                    _sqLiteConnection.DeleteAll<GameAnalyticsEntry>();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"清除所有日志失败: {ex.Message}");
                }
            }
        }
    }
}