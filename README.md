# JT808Gateway

基于DotNetty封装的JT808DotNetty支持TCP/UDP通用消息业务处理

基于Pipeline封装的JT808Pipeline支持TCP/UDP通用消息业务处理

[了解JT808协议进这边](https://github.com/SmallChi/JT808)

[了解JT809协议进这边](https://github.com/SmallChi/JT809)

[了解JT1078协议进这边](https://github.com/SmallChi/JT1078)

[了解JTNE协议进这边](https://github.com/SmallChi/JTNewEnergy)

 [玩一玩压力测试](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/README.md)

[![MIT Licence](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/SmallChi/JT808DotNetty/blob/master/LICENSE)[![Build Status](https://travis-ci.org/SmallChi/JT808DotNetty.svg?branch=master)](https://travis-ci.org/SmallChi/JT808DotNetty)

## 新网关的优势

1. 跨平台
2. 借助 .NET Core模块化的思想
3. 单机同时一万辆车在线不是梦(真有一万辆车那都很吃香了<(￣3￣)> <(￣3￣)> <(￣3￣)>  )
4. 简单易上手

## 设计模型

![design_model](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/img/design_model.png)

## 集成接口功能（JT808.DotNetty.Abstractions）

|接口名称|接口说明|使用场景|
|:------:|:------|:------|
| IJT808SessionProducer| 会话通知（在线/离线）数据生产接口| 有些超长待机的设备，不会实时保持连接，那么通过平台下发的命令是无法到达的，这时候就需要设备一上线，就即时通知服务去处理，然后在即时的下发消息到设备。|
| IJT808SessionConsumer| 会话通知（在线/离线）数据消费接口| -|
| IJT808MsgProducer| 数据生产接口| 网关将接收到的数据发送到队列|
| IJT808MsgConsumer| 数据消费接口| 将数据进行对应的消息业务处理(例：设备流量统计、第三方平台数据转发、消息日志等) |
| IJT808MsgReplyProducer| 应答数据生产接口|将生产的数据解析为对应的消息Id应答发送到队列 |
| IJT808MsgReplyConsumer| 应答数据消费接口| 将接收到的应答数据下发给设备|

> 使用物联网卡通过udp下发指令时，存储的那个socket地址端口，有效期非常短,不速度快点下发，那个socket地址端口就可能映射到别的对应卡去了,所以此处采用跟随设备消息下发指令。

## 基于网关的相关服务

|服务名称|服务说明|使用场景|
|:------:|:------|:------|
|MsgIdHandler| 消息处理服务|从队列中消费设备上报数据，再结合自身的业务场景，将数据进行处理并入库 |
|MsgLogging | 消息日志服务|从队列中消费设备上报和平台应答数据，再将数据存入influxdb等数据库中，便于技术和技术支持排查设备与平台交互的原始数据|
|ReplyMessage| 消息响应服务| 用于响应设备上报消息，以及下发指令信息到设备|
|SessionNotice| 会话管理服务| 通知设备上线下线，对于udp设备来说，可以在设备上线时，将指令跟随消息下发到设备|
|Traffic|流量统计服务 |由于运营商sim卡查询流量滞后，通过流量统计服务可以实时准确的统计设备流量，可以最优配置设备的流量大小，以节省成本
|Transmit| 原包转发服务|该服务可以将设备上报原始数据转发到第三方，支持全部转发，指定终端号转发|

## 基于WebApi的消息业务处理程序（JT808.DotNetty.WebApi）

通过继承JT808.DotNetty.Core.Handlers.JT808MsgIdHttpHandlerBase去实现自定义的WebApi接口服务。

## 基于GRPC的消息业务处理程序

[GRPC协议](https://github.com/SmallChi/JT808Gateway/blob/master/src/JT808.Gateway.Abstractions/Protos/JT808Gateway.proto)

## 基于DotNetty的NuGet安装

| Package Name          | Version                                            | Downloads                                           |
| --------------------- | -------------------------------------------------- | --------------------------------------------------- |
| Install-Package JT808.DotNetty.Abstractions | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Abstractions.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Abstractions.svg) |
| Install-Package JT808.DotNetty.Core | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Core.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Core.svg) |
| Install-Package JT808.DotNetty.Tcp | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Tcp.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Tcp.svg) |
| Install-Package JT808.DotNetty.Udp | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Udp.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Udp.svg) |
| Install-Package JT808.DotNetty.WebApi | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.WebApi.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApi.svg) |
| Install-Package JT808.DotNetty.WebApiClientTool | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.WebApiClientTool.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApiClientTool.svg) |
| Install-Package JT808.DotNetty.Client | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Client.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Client.svg) |
| Install-Package JT808.DotNetty.Transmit | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Transmit.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Transmit.svg) |
| Install-Package JT808.DotNetty.Traffic | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Traffic.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Traffic.svg)|
| Install-Package JT808.DotNetty.SessionNotice | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.SessionNotice.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.SessionNotice.svg)|
| Install-Package JT808.DotNetty.ReplyMessage | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.ReplyMessage.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.ReplyMessage.svg)|
| Install-Package JT808.DotNetty.MsgLogging | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.MsgLogging.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.MsgLogging.svg)|
| Install-Package JT808.DotNetty.MsgIdHandler | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.MsgIdHandler.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.MsgIdHandler.svg)|
| Install-Package JT808.DotNetty.Kafka | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Kafka.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Kafka.svg) |
| Install-Package JT808.DotNetty.RabbitMQ | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.RabbitMQ.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.RabbitMQ.svg) |

## 基于core 3.1 Pipeline的NuGet安装

| Package Name          | Version                                            | Downloads                                           |
| --------------------- | -------------------------------------------------- | --------------------------------------------------- |
| Install-Package JT808.Gateway.Abstractions| ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/v/JT808.Gateway.Abstractions.svg) | ![JT808.Gateway.Abstractions](https://img.shields.io/nuget/dt/JT808.Gateway.Abstractions.svg) |
| Install-Package JT808.Gateway | ![JT808.Gateway](https://img.shields.io/nuget/v/JT808.Gateway.svg) | ![JT808.Gateway](https://img.shields.io/nuget/dt/JT808.Gateway.svg) |
| Install-Package JT808.Gateway.Kafka| ![JT808.Gateway.Kafka](https://img.shields.io/nuget/v/JT808.Gateway.Kafka.svg) | ![JT808.Gateway.Kafka](https://img.shields.io/nuget/dt/JT808.Gateway.Kafka.svg) |
| Install-Package JT808.Gateway.Transmit | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.Transmit.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.Transmit.svg) |
| Install-Package JT808.Gateway.Traffic | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.Traffic.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.Traffic.svg)|
| Install-Package JT808.Gateway.SessionNotice | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.SessionNotice.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.SessionNotice.svg)|
| Install-Package JT808.Gateway.ReplyMessage | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.ReplyMessage.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.ReplyMessage.svg)|
| Install-Package JT808.Gateway.MsgLogging | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.MsgLogging.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.MsgLogging.svg)|
| Install-Package JT808.Gateway.MsgIdHandler | ![JT808](https://img.shields.io/nuget/v/JT808.Gateway.MsgIdHandler.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.Gateway.MsgIdHandler.svg)|

## 举个栗子1

``` demo1
static async Task Main(string[] args)
{
    var serverHostBuilder = new HostBuilder()
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureLogging((context, logging) =>
        {
            logging.AddConsole();  
            logging.SetMinimumLevel(LogLevel.Trace);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddJT808Configure()
                    .AddJT808NettyCore(hostContext.Configuration)
                    .AddJT808TcpNettyHost()
                    .AddJT808UdpNettyHost()
                    .AddJT808WebApiNettyHost()
                    //扩展webapi JT808MsgIdHttpHandlerBase
                    //.ReplaceMsgIdHandler<JT808MsgIdHttpCustomHandler>()
                    .Builder();
                    //添加kafka插件
                    //.AddJT808ServerKafkaMsgProducer(hostContext.Configuration)
                    //.AddJT808ServerKafkaMsgReplyConsumer(hostContext.Configuration)
                    //.AddJT808ServerKafkaSessionProducer(hostContext.Configuration)
                    //.Builder();
                    //使用微软自带的webapi客户端
                    //services.AddHttpClient("jt808webapi", c =>
                    //{
                    //    c.BaseAddress = new Uri("http://localhost:828/");
                    //    c.DefaultRequestHeaders.Add("token", "123456);
                    //})
                    //.AddTypedClient<JT808HttpClient>();
                    //var client = services.BuildServiceProvider().GetRequiredService<JT808HttpClient>();
                    //var result = client.GetTcpAtomicCounter();
        });

    await serverHostBuilder.RunConsoleAsync();
}
```

如图所示：
![demo1](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/demo1.png)

## 举个栗子2

``` 1
static async Task Main(string[] args)
{
    var serverHostBuilder = new HostBuilder()
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{ hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        })
        .ConfigureLogging((context, logging) =>
        {
            Console.WriteLine($"Environment.OSVersion.Platform:{Environment.OSVersion.Platform.ToString()}");
            NLog.LogManager.LoadConfiguration($"Configs/nlog.{Environment.OSVersion.Platform.ToString()}.config");
            logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            logging.SetMinimumLevel(LogLevel.Trace);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddJT808Configure()
                    .AddJT808Gateway()  
                    .AddTcp()
                    .AddUdp()
                    .AddGrpc();
            //services.AddHostedService<CallGrpcClientJob>();
        });

    await serverHostBuilder.RunConsoleAsync();
}
```

## 举个栗子3

1.打开项目进行还原编译生成

2.进入JT808.DotNetty.SimpleServer项目下的Debug目录运行服务端

3.进入JT808.DotNetty.SimpleClient项目下的Debug目录运行客户端

如图所示：
![demo2](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/demo2.png)
