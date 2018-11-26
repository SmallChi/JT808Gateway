# JT808DotNetty

基于DotNetty封装的JT808DotNetty专注消息业务处理

## 集成功能实现

### 1.集成原包分发器

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| ISourcePackageDispatcher | 原包分发器(支持热更新) | 需要将源数据转给其他平台 |

### 2.集成WebApi服务器（默认端口828）

#### 2.1.统一下发设备消息服务 IJT808UnificationSendService

#### 2.2.管理会话服务 IJT808SessionService

#### 2.3.消息包计数服务 JT808AtomicCounterService

### 3.集成业务消息处理程序

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| JT808MsgIdHandlerBase | 业务消息处理程序 | 需要自定义实现业务消息处理程序 |