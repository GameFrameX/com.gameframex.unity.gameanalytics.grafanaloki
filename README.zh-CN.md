<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX GameAnalytics Grafana Loki

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

这个包提供了基于 Grafana Loki 的日志打点系统，实现了 GameFrameX.GameAnalytics 接口。它使游戏开发者能够将分析事件发送到 Grafana Loki，实现集中式日志管理和可视化。

## 特性

- 完全兼容 GameFrameX.GameAnalytics 接口
- 支持本地日志存储和批量发送
- 定时发送机制
- 网络状态感知和失败重试
- 可配置的 Loki 服务器地址和批处理参数

## 安装

### 依赖

本包依赖 `com.gilzoide.sqlite-net`。请使用 openupm-cli 安装：

```
openupm add com.gilzoide.sqlite-net
```

或者在 `Packages/manifest.json` 中添加以下 scope 和 dependency：

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://gameframex.upm.alianblank.uk",
      "scopes": [
        "com.gameframex"
      ]
    }
  ],
  "dependencies": {
    "com.gilzoide.sqlite-net": "1.2.4"
  }
}
```

### 通过 Git URL 安装（推荐）

1. 在 Unity 编辑器中打开 Package Manager
2. 点击 "+" 按钮选择 "Add package from git URL"
3. 输入以下 URL：
   ```
   https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git
   ```

### 通过 manifest.json 安装

在项目的 `Packages/manifest.json` 文件中添加：

```json
{
  "dependencies": {
    "com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git"
  }
}
```

### 手动安装

1. 下载最新版本发布包
2. 解压到项目的 `Packages` 目录下
3. Unity 会自动识别并加载包

## 快速开始

1. 在 Unity 项目中引入包
2. 在 GameAnalyticsComponent 中添加 GrafanaLokiAnalyticsManager 配置
3. 设置 Loki 服务器 URL 和其他配置参数
4. 使用现有的 AnalyticsManager.SendAnalyticsEvent 方法发送事件

### 示例配置

```json
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

- `LokiUrl`：Grafana Loki 服务器的 URL
- `BatchSendIntervalSeconds`：批量发送日志的间隔时间（秒）
- `MaxBatchSize`：每批发送的最大日志数量
- `StorageType`：日志存储类型，可选值：`File` 或 `PlayerPrefs`
- `LogFilePath`：当 StorageType 为 File 时，日志文件的存储路径

## 更新日志

详见 [CHANGELOG.md](CHANGELOG.md)。

## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
