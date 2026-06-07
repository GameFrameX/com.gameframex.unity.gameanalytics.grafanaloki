<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX GameAnalytics Grafana Loki

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## 項目簡介

這個包提供了基於 Grafana Loki 的日誌打點系統，實現了 GameFrameX.GameAnalytics 介面。它使遊戲開發者能夠將分析事件傳送到 Grafana Loki，實現集中式日誌管理和視覺化。

## 特性

- 完全相容 GameFrameX.GameAnalytics 介面
- 支援本機日誌儲存和批次傳送
- 定時傳送機制
- 網路狀態感知和失敗重試
- 可設定的 Loki 伺服器位址和批次處理參數

## 安裝

### 依賴

本包依賴 `com.gilzoide.sqlite-net`。請使用 openupm-cli 安裝：

```
openupm add com.gilzoide.sqlite-net
```

或者在 `Packages/manifest.json` 中新增以下 scope 和 dependency：

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

### 透過 Git URL 安裝（推薦）

1. 在 Unity 編輯器中開啟 Package Manager
2. 點擊 "+" 按鈕選擇 "Add package from git URL"
3. 輸入以下 URL：
   ```
   https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git
   ```

### 透過 manifest.json 安裝

在專案的 `Packages/manifest.json` 檔案中新增：

```json
{
  "dependencies": {
    "com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git"
  }
}
```

### 手動安裝

1. 下載最新版本發佈包
2. 解壓縮到專案的 `Packages` 目錄下
3. Unity 會自動辨識並載入包

## 快速開始

1. 在 Unity 專案中引入包
2. 在 GameAnalyticsComponent 中新增 GrafanaLokiAnalyticsManager 設定
3. 設定 Loki 伺服器 URL 和其他設定參數
4. 使用現有的 AnalyticsManager.SendAnalyticsEvent 方法傳送事件

### 範例設定

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

## 設定參數

- `LokiUrl`：Grafana Loki 伺服器的 URL
- `BatchSendIntervalSeconds`：批次傳送日誌的間隔時間（秒）
- `MaxBatchSize`：每批傳送的最大日誌數量
- `StorageType`：日誌儲存類型，可選值：`File` 或 `PlayerPrefs`
- `LogFilePath`：當 StorageType 為 File 時，日誌檔案的儲存路徑

## 更新日誌

詳見 [CHANGELOG.md](CHANGELOG.md)。

## 開源協議

本專案基於 MIT 協議開源，詳見 [LICENSE.md](LICENSE.md)。
