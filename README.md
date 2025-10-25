# Game Frame X Game Analytics Grafana Loki

这个包提供了基于Grafana Loki的日志打点系统，实现了GameFrameX.GameAnalytics接口。

## 特性

- 完全兼容GameFrameX.GameAnalytics接口
- 支持本地日志存储和批量发送
- 定时发送机制
- 网络状态感知和失败重试
- 可配置的Loki服务器地址和批处理参数

## 安装

### 依赖

本包依赖 `com.gilzoide.sqlite-net`。请使用 openupm-cli 安装:

```
openupm add com.gilzoide.sqlite-net
```

或者, 在 `Packages/manifest.json` 中添加以下 scope 和 dependency:

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.gilzoide"
      ]
    }
  ],
  "dependencies": {
    "com.gilzoide.sqlite-net": "1.2.4"
  }
}
```

# 使用方式(任选其一)

1. 直接在 `manifest.json` 的文件中的 `dependencies` 节点下添加以下内容
   ```json
      {"com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki.git"}
    ```
2. 在Unity 的`Packages Manager` 中使用`Git URL` 的方式添加库,地址为：https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki.git

3. 直接下载仓库放置到Unity 项目的`Packages` 目录下。会自动加载识别

## 使用方法

1. 在Unity项目中引入包
2. 在GameAnalyticsComponent中添加GrafanaLokiAnalyticsManager配置
3. 设置Loki服务器URL和其他配置参数
4. 使用现有的AnalyticsManager.SendAnalyticsEvent方法发送事件

### 示例配置

```csharp
// 在Resources/GameAnalytics/GameAnalyticsSettings.asset中配置
{
  "ComponentType": "GameFrameX.GameAnalytics.GrafanaLoki.Runtime.GrafanaLokiAnalyticsManager",
  "Setting": {
    "LokiUrl": "http://your-loki-server:3100/loki/api/v1/push",
    "BatchSendIntervalSeconds": "5",
    "MaxBatchSize": "100",
    "StorageType": "File",
    "LogFilePath": "Logs"
  }
}
```

## 配置参数

- `LokiUrl`: Grafana Loki服务器的URL
- `BatchSendIntervalSeconds`: 批量发送日志的间隔时间（秒）
- `MaxBatchSize`: 每批发送的最大日志数量
- `StorageType`: 日志存储类型，可选值：`File`或`PlayerPrefs`
- `LogFilePath`: 当StorageType为File时，日志文件的存储路径

## 许可证

[MIT](LICENSE.md)