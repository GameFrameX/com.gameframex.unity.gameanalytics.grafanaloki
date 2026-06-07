<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX GameAnalytics Grafana Loki

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · [QQグループ](https://qm.qq.com/q/5U9Fvebw)

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>
## プロジェクト概要

このパッケージは、Grafana Loki ベースのログアナリティクスシステムを提供し、GameFrameX.GameAnalytics インターフェースを実装しています。ゲーム開発者がアナリティクスイベントを Grafana Loki に送信し、一元化されたログ管理と可視化を実現できます。

## 特徴

- GameFrameX.GameAnalytics インターフェースと完全互換
- ローカルログストレージとバッチ送信
- 定期送信メカニズム
- ネットワークステータス検知とリトライ
- 設定可能な Loki サーバー URL とバッチ処理パラメータ

## インストール

### 依存関係

このパッケージは `com.gilzoide.sqlite-net` に依存しています。openupm-cli でインストールしてください：

```
openupm add com.gilzoide.sqlite-net
```

または、`Packages/manifest.json` に以下のスコープと依存関係を追加：

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

### Git URL 経由（推奨）

1. Unity エディタで Package Manager を開く
2. "+" ボタンをクリックし "Add package from git URL" を選択
3. 以下の URL を入力：
   ```
   https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git
   ```

### manifest.json 経由

プロジェクトの `Packages/manifest.json` に以下を追加：

```json
{
  "dependencies": {
    "com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git"
  }
}
```

### 手動インストール

1. 最新のリリースパッケージをダウンロード
2. プロジェクトの `Packages` ディレクトリに展開
3. Unity が自動的にパッケージを認識して読み込みます

## クイックスタート

1. Unity プロジェクトにパッケージをインポート
2. GameAnalyticsComponent に GrafanaLokiAnalyticsManager の設定を追加
3. Loki サーバーの URL とその他の設定パラメータを指定
4. 既存の AnalyticsManager.SendAnalyticsEvent メソッドを使用してイベントを送信

### 設定例

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

## 設定パラメータ

- `LokiUrl`：Grafana Loki サーバーの URL
- `BatchSendIntervalSeconds`：バッチ送信の間隔（秒）
- `MaxBatchSize`：バッチあたりの最大ログ数
- `StorageType`：ログストレージタイプ、オプション：`File` または `PlayerPrefs`
- `LogFilePath`：StorageType が File の場合のログファイルの保存パス

## 変更履歴

詳細は [CHANGELOG.md](CHANGELOG.md) をご覧ください。

## ライセンス

このプロジェクトは MIT ライセンスの下で公開されています。詳細は [LICENSE.md](LICENSE.md) をご覧ください。
