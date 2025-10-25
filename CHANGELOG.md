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