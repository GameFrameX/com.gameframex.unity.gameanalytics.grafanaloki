using GameFrameX.GameAnalytics.Editor;
using GameFrameX.GameAnalytics.GrafanaLoki.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.GameAnalytics.GrafanaLoki.Editor
{
    /// <summary>
    /// Grafana Loki分析组件检查器
    /// </summary>
    [CustomEditor(typeof(GrafanaLokiAnalyticsSetting))]
    public class GrafanaLokiAnalyticsComponentInspector : UnityEditor.Editor
    {
        private SerializedProperty _lokiUrlProperty;
        private SerializedProperty _batchSendIntervalSecondsProperty;
        private SerializedProperty _maxBatchSizeProperty;
        private SerializedProperty _storageTypeProperty;
        private SerializedProperty _logFilePathProperty;
        private SerializedProperty _defaultLabelsProperty;

        private void OnEnable()
        {
            _lokiUrlProperty = serializedObject.FindProperty("LokiUrl");
            _batchSendIntervalSecondsProperty = serializedObject.FindProperty("BatchSendIntervalSeconds");
            _maxBatchSizeProperty = serializedObject.FindProperty("MaxBatchSize");
            _storageTypeProperty = serializedObject.FindProperty("StorageType");
            _logFilePathProperty = serializedObject.FindProperty("LogFilePath");
            _defaultLabelsProperty = serializedObject.FindProperty("DefaultLabels");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Grafana Loki设置", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Loki服务器URL
            EditorGUILayout.PropertyField(_lokiUrlProperty, new GUIContent("Loki服务器URL", "Grafana Loki服务器的URL地址"));

            // 批量发送间隔
            EditorGUILayout.PropertyField(_batchSendIntervalSecondsProperty, new GUIContent("批量发送间隔（秒）", "日志批量发送的时间间隔"));
            _batchSendIntervalSecondsProperty.intValue = Mathf.Max(1, _batchSendIntervalSecondsProperty.intValue);

            // 最大批量大小
            EditorGUILayout.PropertyField(_maxBatchSizeProperty, new GUIContent("最大批量大小", "每批发送的最大日志数量"));
            _maxBatchSizeProperty.intValue = Mathf.Max(1, _maxBatchSizeProperty.intValue);

            // 存储类型
            string[] storageTypes = new string[] { "File", "PlayerPrefs" };
            int selectedIndex = _storageTypeProperty.stringValue == "PlayerPrefs" ? 1 : 0;
            selectedIndex = EditorGUILayout.Popup(new GUIContent("存储类型", "日志本地存储的类型"), selectedIndex, storageTypes);
            _storageTypeProperty.stringValue = storageTypes[selectedIndex];

            // 日志文件路径（仅当存储类型为File时显示）
            if (_storageTypeProperty.stringValue == "File")
            {
                EditorGUILayout.PropertyField(_logFilePathProperty, new GUIContent("日志文件路径", "日志文件的存储路径（相对于Application.persistentDataPath）"));
            }

            // 默认标签
            EditorGUILayout.PropertyField(_defaultLabelsProperty, new GUIContent("默认标签", "发送到Loki的默认标签"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}