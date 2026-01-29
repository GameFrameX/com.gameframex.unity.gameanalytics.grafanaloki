## [1.0.1](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/compare/1.0.0...1.0.1) (2026-01-29)


### Bug Fixes

* **Network:** 修复LokiHttpClient构造函数中headers的引用问题 ([d4ec500](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/commit/d4ec5006cb3eb7f30a2091da3ab7b60a8899373b))
* 修复事件数据与公共属性字典引用共享的问题 ([529e5db](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/commit/529e5db03b7dbf4523e18e928c31baa6072ddcbf))
* 修复自定义字段字典被意外修改的问题 ([320dc52](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/commit/320dc5262003f400f4d5c79b8112f64b75981776))

# 1.0.0 (2025-12-24)


### Features

* **ci:** change ci ([f7fdb79](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/commit/f7fdb79087115be709d7c10aaf6378af207ca0ec))
* 添加Grafana Loki游戏分析组件实现 ([bb4e8f9](https://github.com/gameframex/com.gameframex.unity.gameanalytics.grafanaloki/commit/bb4e8f9ca807c68a0c900aab42bee7bde2f89f89))

# 更新日志

所有对此项目的显著更改都将记录在此文件中。

格式基于[Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
并且此项目遵循[语义化版本](https://semver.org/lang/zh-CN/)。

## [1.0.0] - 2023-12-01

### 新增

- 初始版本发布
- 实现基于Grafana Loki的日志打点系统
- 支持本地日志存储和批量发送
- 定时发送机制
- 网络状态感知和失败重试
- 可配置的Loki服务器地址和批处理参数
