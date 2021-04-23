# JT808Gateway

基于Pipeline封装的JT808Pipeline支持TCP/UDP通用消息业务处理

基于DotNetty封装的JT808DotNetty支持TCP/UDP通用消息业务处理

[了解JT808协议进这边](https://github.com/SmallChi/JT808)

[了解JT809协议进这边](https://github.com/SmallChi/JT809)

[了解JT1078协议进这边](https://github.com/SmallChi/JT1078)

[了解JTNE协议进这边](https://github.com/SmallChi/JTNewEnergy)

 [玩一玩压力测试](https://github.com/SmallChi/JT808Gateway/blob/master/doc/README.md)

[![MIT Licence](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/SmallChi/JT808Gateway/blob/master/LICENSE)![.NET Core](https://github.com/SmallChi/JT808Gateway/workflows/.NET%20Core/badge.svg?branch=master)

## 新网关的优势

1. 跨平台
2. 借助 .NET Core模块化的思想
3. 单机同时一万辆车在线不是梦(真有一万辆车那都很吃香了<(￣3￣)> <(￣3￣)> <(￣3￣)>  )
4. 简单易上手

## 设计模型

![design_model](https://github.com/SmallChi/JT808Gateway/blob/master/doc/img/design_model.png)

## 集成接口功能

|           接口名称            | 接口说明                          | 使用场景                                                                                                                                             |
| :--------------------------- | :-------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------- |
|     IJT808SessionProducer     | 会话通知（在线/离线）数据生产接口 | 有些超长待机的设备，不会实时保持连接，那么通过平台下发的命令是无法到达的，这时候就需要设备一上线，就即时通知服务去处理，然后在即时的下发消息到设备。 |
|     IJT808SessionConsumer     | 会话通知（在线/离线）数据消费接口 | -                                                                                                                                                    |
|       IJT808MsgProducer       | 数据生产接口                      | 网关将接收到的数据发送到队列                                                                                                                         |
|       IJT808MsgConsumer       | 数据消费接口                      | 将数据进行对应的消息业务处理(例：设备流量统计、第三方平台数据转发、消息日志等)                                                                       |
|    IJT808MsgReplyProducer     | 应答数据生产接口                  | 将生产的数据解析为对应的消息Id应答发送到队列                                                                                                         |
|    IJT808MsgReplyConsumer     | 应答数据消费接口                  | 将接收到的应答数据下发给设备                                                                                                                         |
| IJT808MsgReplyLoggingProducer | 网关应答数据日志生产接口          | 将网关能解析到直接能下发的数据发送到队列                                                                                                             |
| IJT808MsgReplyLoggingConsumer | 网关应答数据日志消费接口          | 将网关能解析到直接能下发的数据发送到日志系统                                                                                                         |

> 使用物联网卡通过udp下发指令时，存储的那个socket地址端口，有效期非常短,不速度快点下发，那个socket地址端口就可能映射到别的对应卡去了,所以此处采用跟随设备消息下发指令。

## 基于网关的相关服务

|              服务名称              | 服务说明     | 使用场景                                                                                                           |
| :-------------------------------- | :----------- | :----------------------------------------------------------------------------------------------------------------- |
|            MsgIdHandler            | 消息处理服务 | 从队列中消费设备上报数据，再结合自身的业务场景，将数据进行处理并入库                                               |
|             MsgLogging             | 消息日志服务 | 从队列中消费设备上报和平台应答数据，再将数据存入influxdb等数据库中，便于技术和技术支持排查设备与平台交互的原始数据 |
|            ReplyMessage            | 消息响应服务 | 用于响应设备上报消息，以及下发指令信息到设备                                                                       |
|           SessionNotice            | 会话管理服务 | 通知设备上线下线，对于udp设备来说，可以在设备上线时，将指令跟随消息下发到设备                                      |
| Traffic (v1.1.0新版pipeline已移出) | 流量统计服务 | 由于运营商sim卡查询流量滞后，通过流量统计服务可以实时准确的统计设备流量，可以最优配置设备的流量大小，以节省成本    |
|              Transmit              | 原包转发服务 | 该服务可以将设备上报原始数据转发到第三方，支持全部转发，指定终端号转发                                             |

## 基于WebApi的消息业务处理程序

通过继承JT808.DotNetty.Core.Handlers.JT808MsgIdHttpHandlerBase去实现自定义的WebApi接口服务。

通过继承JT808.Gateway.Handlers.JT808MsgIdDefaultWebApiHandler去实现自定义的WebApi接口服务。

[接口文档](https://github.com/SmallChi/JT808Gateway/tree/master/api)

## 基于NET5.0 Pipeline

Pipeline分为两种方式使用，一种是使用队列的方式，一种是网关集成的方式。

| 使用方式     | 特性                                                                               | 备注                                         |
| :----------- | :--------------------------------------------------------------------------------- | :------------------------------------------- |
| 使用队列     | 网关不需要重启，相当于透传数据，设备上来的数据直接入队列，通过服务去处理消息。     | 设备多的可以这样搞，这样关注点在业务上面。   |
| 使用网关集成 | 网关需要根据消息业务的变化去处理，也就意味着更改业务，需要重启网关，但是上手简单。 | 设备少的，开发能力弱的，允许设备丢点数据的。 |

### Pipeline的NuGet安装

| Package Name                                                  | Version                                                                                              | Preview  Version                                                                                        | Downloads                                                                                             |
| ------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| Install-Package JT808.Gateway.Abstractions                    | ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/v/JT808.Gateway.Abstractions.svg)         | ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/vpre/JT808.Gateway.Abstractions.svg)         | ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/dt/JT808.Gateway.Abstractions.svg)         | ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/dt/JT808.Gateway.Abstractions.svg) |
| Install-Package JT808.Gateway                                 | ![JT808.Gateway](https://img.shields.io/nuget/v/JT808.Gateway.svg)                                   | ![JT808.Gateway](https://img.shields.io/nuget/vpre/JT808.Gateway.svg)                                   | ![JT808.Gateway](https://img.shields.io/nuget/dt/JT808.Gateway.svg)                                   |
| Install-Package JT808.Gateway.WebApiClientTool                | ![JT808.Gateway.WebApiClientTool](https://img.shields.io/nuget/v/JT808.Gateway.WebApiClientTool.svg) | ![JT808.Gateway.WebApiClientTool](https://img.shields.io/nuget/vpre/JT808.Gateway.WebApiClientTool.svg) | ![JT808.Gateway.WebApiClientTool](https://img.shields.io/nuget/dt/JT808.Gateway.WebApiClientTool.svg) |
| Install-Package JT808.Gateway.Client                          | ![JT808.Gateway.Client](https://img.shields.io/nuget/v/JT808.Gateway.Client.svg)                     | ![JT808.Gateway.Client](https://img.shields.io/nuget/vpre/JT808.Gateway.Client.svg)                     | ![JT808.Gateway.Client](https://img.shields.io/nuget/dt/JT808.Gateway.Client.svg)                     |
| Install-Package JT808.Gateway.Kafka                           | ![JT808.Gateway.Kafka](https://img.shields.io/nuget/v/JT808.Gateway.Kafka.svg)                       | ![JT808.Gateway.Kafka](https://img.shields.io/nuget/vpre/JT808.Gateway.Kafka.svg)                       | ![JT808.Gateway.Kafka](https://img.shields.io/nuget/dt/JT808.Gateway.Kafka.svg)                       |
| Install-Package JT808.Gateway.Transmit                        | ![JT808.Gateway.Transmit](https://img.shields.io/nuget/v/JT808.Gateway.Transmit.svg)                 | ![JT808.Gateway.Transmit](https://img.shields.io/nuget/vpre/JT808.Gateway.Transmit.svg)                 | ![JT808.Gateway.Transmit](https://img.shields.io/nuget/dt/JT808.Gateway.Transmit.svg)                 |
| Install-Package JT808.Gateway.SessionNotice                   | ![JT808.Gateway.SessionNotice](https://img.shields.io/nuget/v/JT808.Gateway.SessionNotice.svg)       | ![JT808.Gateway.SessionNotice](https://img.shields.io/nuget/vpre/JT808.Gateway.SessionNotice.svg)       | ![JT808.Gateway.SessionNotice](https://img.shields.io/nuget/dt/JT808.Gateway.SessionNotice.svg)       |
| Install-Package JT808.Gateway.ReplyMessage                    | ![JT808.Gateway.ReplyMessage](https://img.shields.io/nuget/v/JT808.Gateway.ReplyMessage.svg)         | ![JT808.Gateway.ReplyMessage](https://img.shields.io/nuget/vpre/JT808.Gateway.ReplyMessage.svg)         | ![JT808.Gateway.ReplyMessage](https://img.shields.io/nuget/dt/JT808.Gateway.ReplyMessage.svg)         |
| Install-Package JT808.Gateway.MsgLogging                      | ![JT808.Gateway.MsgLogging](https://img.shields.io/nuget/v/JT808.Gateway.MsgLogging.svg)             | ![JT808.Gateway.MsgLogging](https://img.shields.io/nuget/vpre/JT808.Gateway.MsgLogging.svg)             | ![JT808.Gateway.MsgLogging](https://img.shields.io/nuget/dt/JT808.Gateway.MsgLogging.svg)             |
| Install-Package JT808.Gateway.MsgIdHandler                    | ![JT808.Gateway.MsgIdHandler](https://img.shields.io/nuget/v/JT808.Gateway.MsgIdHandler.svg)         | ![JT808.Gateway.MsgIdHandler](https://img.shields.io/nuget/vpre/JT808.Gateway.MsgIdHandler.svg)         | ![JT808.Gateway.MsgIdHandler](https://img.shields.io/nuget/dt/JT808.Gateway.MsgIdHandler.svg)         |

## 基于DotNetty

### DotNetty的NuGet安装

| Package Name| Version |Preview  Version | Downloads|
| --- | --- | ---| ---|
| Install-Package JT808.DotNetty.Abstractions     | ![JT808.DotNetty.Abstractions](https://img.shields.io/nuget/v/JT808.DotNetty.Abstractions.svg)| ![JT808.DotNetty.Abstractions](https://img.shields.io/nuget/vpre/JT808.DotNetty.Abstractions.svg)                 | ![JT808.DotNetty.Abstractions](https://img.shields.io/nuget/dt/JT808.DotNetty.Abstractions.svg)         |
| Install-Package JT808.DotNetty.Core             | ![JT808.DotNetty.Core](https://img.shields.io/nuget/v/JT808.DotNetty.Core.svg)   | ![JT808.DotNetty.Core](https://img.shields.io/nuget/vpre/JT808.DotNetty.Core.svg)                                       | ![JT808.DotNetty.Core](https://img.shields.io/nuget/dt/JT808.DotNetty.Core.svg)                         |
| Install-Package JT808.DotNetty.Tcp              | ![JT808.DotNetty.Tcp](https://img.shields.io/nuget/v/JT808.DotNetty.Tcp.svg)     | ![JT808.DotNetty.Tcp](https://img.shields.io/nuget/vpre/JT808.DotNetty.Tcp.svg)                                       | ![JT808.DotNetty.Tcp](https://img.shields.io/nuget/dt/JT808.DotNetty.Tcp.svg)                           |
| Install-Package JT808.DotNetty.Udp              | ![JT808.DotNetty.Udp](https://img.shields.io/nuget/v/JT808.DotNetty.Udp.svg)    | ![JT808.DotNetty.Udp](https://img.shields.io/nuget/vpre/JT808.DotNetty.Udp.svg)                                        | ![JT808.DotNetty.Udp](https://img.shields.io/nuget/dt/JT808.DotNetty.Udp.svg)                           |
| Install-Package JT808.DotNetty.WebApi           | ![JT808.DotNetty.WebApi](https://img.shields.io/nuget/v/JT808.DotNetty.WebApi.svg)   | ![JT808.DotNetty.WebApi](https://img.shields.io/nuget/vpre/JT808.DotNetty.WebApi.svg)                                   | ![JT808.DotNetty.WebApi](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApi.svg)                     |
| Install-Package JT808.DotNetty.WebApiClientTool | ![JT808.DotNetty.WebApiClientTool](https://img.shields.io/nuget/v/JT808.DotNetty.WebApiClientTool.svg) | ![JT808.DotNetty.WebApiClientTool](https://img.shields.io/nuget/vpre/JT808.DotNetty.WebApiClientTool.svg)                 | ![JT808.DotNetty.WebApiClientTool](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApiClientTool.svg) |
| Install-Package JT808.DotNetty.Client           | ![JT808.DotNetty.Client](https://img.shields.io/nuget/v/JT808.DotNetty.Client.svg)   | ![JT808.DotNetty.Client](https://img.shields.io/nuget/vpre/JT808.DotNetty.Client.svg)                                   | ![JT808.DotNetty.Client](https://img.shields.io/nuget/dt/JT808.DotNetty.Client.svg)                     |
| Install-Package JT808.DotNetty.Transmit         | ![JT808.DotNetty.Transmit](https://img.shields.io/nuget/v/JT808.DotNetty.Transmit.svg)   | ![JT808.DotNetty.Transmit](https://img.shields.io/nuget/vpre/JT808.DotNetty.Transmit.svg)                               | ![JT808.DotNetty.Transmit](https://img.shields.io/nuget/dt/JT808.DotNetty.Transmit.svg)                 |
| Install-Package JT808.DotNetty.Traffic          | ![JT808.DotNetty.Traffic](https://img.shields.io/nuget/v/JT808.DotNetty.Traffic.svg)   | ![JT808.DotNetty.Traffic](https://img.shields.io/nuget/vpre/JT808.DotNetty.Traffic.svg)                                 | ![JT808.DotNetty.Traffic](https://img.shields.io/nuget/dt/JT808.DotNetty.Traffic.svg)                   |
| Install-Package JT808.DotNetty.SessionNotice    | ![JT808.DotNetty.SessionNotice](https://img.shields.io/nuget/v/JT808.DotNetty.SessionNotice.svg)| ![JT808.DotNetty.SessionNotice](https://img.shields.io/nuget/vpre/JT808.DotNetty.SessionNotice.svg)                        | ![JT808.DotNetty.SessionNotice](https://img.shields.io/nuget/dt/JT808.DotNetty.SessionNotice.svg)       |
| Install-Package JT808.DotNetty.ReplyMessage     | ![JT808.DotNetty.ReplyMessage](https://img.shields.io/nuget/v/JT808.DotNetty.ReplyMessage.svg) | ![JT808.DotNetty.ReplyMessage](https://img.shields.io/nuget/vpre/JT808.DotNetty.ReplyMessage.svg)                         | ![JT808.DotNetty.ReplyMessage](https://img.shields.io/nuget/dt/JT808.DotNetty.ReplyMessage.svg)         |
| Install-Package JT808.DotNetty.MsgLogging       | ![JT808.DotNetty.MsgLogging](https://img.shields.io/nuget/v/JT808.DotNetty.MsgLogging.svg)  | ![JT808.DotNetty.Abstractions](https://img.shields.io/nuget/vpre/JT808.DotNetty.MsgLogging.svg)                            | ![JT808.DotNetty.MsgLogging](https://img.shields.io/nuget/dt/JT808.DotNetty.MsgLogging.svg)             || ![JT808.DotNetty.MsgLogging](https://img.shields.io/nuget/vpre/JT808.DotNetty.MsgLogging.svg)
| Install-Package JT808.DotNetty.MsgIdHandler     | ![JT808.DotNetty.MsgIdHandler](https://img.shields.io/nuget/v/JT808.DotNetty.MsgIdHandler.svg)  | ![JT808.DotNetty.MsgIdHandler](https://img.shields.io/nuget/vpre/JT808.DotNetty.MsgIdHandler.svg)                        | ![JT808.DotNetty.MsgIdHandler](https://img.shields.io/nuget/dt/JT808.DotNetty.MsgIdHandler.svg)         |
| Install-Package JT808.DotNetty.Kafka            | ![JT808.DotNetty.Kafka](https://img.shields.io/nuget/v/JT808.DotNetty.Kafka.svg)    | ![JT808.DotNetty.Kafka](https://img.shields.io/nuget/vpre/JT808.DotNetty.Kafka.svg)                                    | ![JT808.DotNetty.Kafka](https://img.shields.io/nuget/dt/JT808.DotNetty.Kafka.svg)    | ![JT808.DotNetty.Kafka](https://img.shields.io/nuget/vpre/JT808.DotNetty.Kafka.svg)                                    |

## 举个栗子

### Pipeline

#### 使用网关集成方式

1.打开/simples/JT808.Simples.sln项目进行还原编译生成

2.进入JT808.Gateway.SimpleServer项目下的Debug目录运行服务端

3.进入JT808.Gateway.SimpleClient项目下的Debug目录运行客户端

如图所示：
![demo3](https://github.com/SmallChi/JT808Gateway/blob/master/doc/img/demo3.png)

#### 使用队列方式

1.打开/simples/JT808.Simples.sln项目进行还原编译生成

2.JT808.Gateway.SimpleQueueServer项目下的Debug目录运行服务端

3.JT808.Gateway.SimpleQueueService项目下的Debug目录运行消息处理服务

4.JT808.Gateway.SimpleQueueNotification项目下的Debug目录运行WebSocket服务
从浏览器中打开localhost:5000查看数据

5.进入JT808.Gateway.SimpleClient项目下的Debug目录运行客户端

> 注意：需要安装kafka和zookeeper

如图所示：
![demo4](https://github.com/SmallChi/JT808Gateway/blob/master/doc/img/demo4.png)

### DotNetty

1.打开/simples/JT808.Simples.sln项目进行还原编译生成

2.进入JT808.DotNetty.SimpleServer项目下的Debug目录运行服务端

3.进入JT808.DotNetty.SimpleClient项目下的Debug目录运行客户端

如图所示：
![demo2](https://github.com/SmallChi/JT808Gateway/blob/master/doc/img/demo2.png)

## 常见问题

- 多协议兼容实现思路[点我查看](https://github.com/SmallChi/JT808Gateway/issues/11#issuecomment-727687417)

> 单端口兼容多协议虽然可以实现，但是还是不建议这么做，建议最好是用端口分开，避免不必要的麻烦