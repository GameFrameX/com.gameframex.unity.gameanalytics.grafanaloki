<div align="center">
  <img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />

  # GameFrameX GameAnalytics Grafana Loki

  [![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/releases)
  [![License](https://img.shields.io/badge/license-MIT-orange.svg)](LICENSE.md)
  [![Documentation](https://img.shields.io/badge/docs-gameframex-blue.svg)](https://gameframex.doc.alianblank.com)

  All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

  [Documentation](https://gameframex.doc.alianblank.com) | [Quick Start](#quick-start)

  **English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)
</div>

---

## Project Overview

This package provides a Grafana Loki-based log analytics system that implements the GameFrameX.GameAnalytics interface. It enables game developers to send analytics events to Grafana Loki for centralized log management and visualization.

## Features

- Fully compatible with GameFrameX.GameAnalytics interface
- Local log storage and batch sending
- Timed sending mechanism
- Network status awareness and failure retry
- Configurable Loki server URL and batch processing parameters

## Installation

### Dependencies

This package depends on `com.gilzoide.sqlite-net`. Install it via openupm-cli:

```
openupm add com.gilzoide.sqlite-net
```

Or add the following scope and dependency in `Packages/manifest.json`:

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

### Via Git URL (Recommended)

1. Open Package Manager in Unity Editor
2. Click the "+" button and select "Add package from git URL"
3. Enter the following URL:
   ```
   https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git
   ```

### Via manifest.json

Add the following to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git"
  }
}
```

### Manual Installation

1. Download the latest release package
2. Extract it to your project's `Packages` directory
3. Unity will automatically recognize and load the package

## Quick Start

1. Import the package into your Unity project
2. Add GrafanaLokiAnalyticsManager configuration to GameAnalyticsComponent
3. Set the Loki server URL and other configuration parameters
4. Use the existing AnalyticsManager.SendAnalyticsEvent method to send events

### Example Configuration

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

## Configuration Parameters

- `LokiUrl`: Grafana Loki server URL
- `BatchSendIntervalSeconds`: Interval for batch sending logs (seconds)
- `MaxBatchSize`: Maximum number of logs per batch
- `StorageType`: Log storage type, options: `File` or `PlayerPrefs`
- `LogFilePath`: Log file storage path when StorageType is File

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for details.

## License

This project is licensed under the MIT License - see [LICENSE.md](LICENSE.md) for details.
