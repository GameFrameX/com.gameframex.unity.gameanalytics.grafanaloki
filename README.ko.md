<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX GameAnalytics Grafana Loki

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki)](https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>
## 프로젝트 개요

이 패키지는 Grafana Loki 기반 로그 분석 시스템을 제공하며, GameFrameX.GameAnalytics 인터페이스를 구현합니다. 게임 개발자가 분석 이벤트를 Grafana Loki로 전송하여 중앙 집중식 로그 관리와 시각화를 실현할 수 있습니다.

## 특징

- GameFrameX.GameAnalytics 인터페이스와 완전 호환
- 로컬 로그 저장 및 배치 전송
- 정기 전송 메커니즘
- 네트워크 상태 인식 및 실패 재시도
- 구성 가능한 Loki 서버 URL 및 배치 처리 매개변수

## 설치

### 종속성

이 패키지는 `com.gilzoide.sqlite-net`에 종속됩니다. openupm-cli로 설치하세요:

```
openupm add com.gilzoide.sqlite-net
```

또는 `Packages/manifest.json`에 다음 스코프와 종속성을 추가:

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

### Git URL을 통해 설치 (권장)

1. Unity 에디터에서 Package Manager 열기
2. "+" 버튼을 클릭하고 "Add package from git URL" 선택
3. 다음 URL 입력:
   ```
   https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git
   ```

### manifest.json을 통해 설치

프로젝트의 `Packages/manifest.json`에 다음을 추가:

```json
{
  "dependencies": {
    "com.gameframex.unity.gameanalytics.grafanaloki": "https://github.com/GameFrameX/com.gameframex.unity.gameanalytics.grafanaloki.git"
  }
}
```

### 수동 설치

1. 최신 릴리스 패키지 다운로드
2. 프로젝트의 `Packages` 디렉토리에 압축 해제
3. Unity가 자동으로 패키지를 인식하고 로드합니다

## 빠른 시작

1. Unity 프로젝트에 패키지 가져오기
2. GameAnalyticsComponent에 GrafanaLokiAnalyticsManager 구성 추가
3. Loki 서버 URL 및 기타 구성 매개변수 설정
4. 기존 AnalyticsManager.SendAnalyticsEvent 메서드를 사용하여 이벤트 전송

### 구성 예시

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

## 구성 매개변수

- `LokiUrl`: Grafana Loki 서버 URL
- `BatchSendIntervalSeconds`: 배치 전송 간격 (초)
- `MaxBatchSize`: 배치당 최대 로그 수
- `StorageType`: 로그 저장 유형, 옵션: `File` 또는 `PlayerPrefs`
- `LogFilePath`: StorageType이 File인 경우 로그 파일 저장 경로

## 변경 로그

자세한 내용은 [CHANGELOG.md](CHANGELOG.md)를 참조하세요.

## 라이선스

이 프로젝트는 MIT 라이선스에 따라 배포됩니다. 자세한 내용은 [LICENSE.md](LICENSE.md)를 참조하세요.
