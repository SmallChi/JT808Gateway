# JT808DotNetty

基于DotNetty封装的JT808DotNetty支持TCP/UDP通用消息业务处理 

[了解JT808协议进这边](https://github.com/SmallChi/JT808)

[了解JT809协议进这边](https://github.com/SmallChi/JT809)

 [玩一玩压力测试](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/README.md)

[![MIT Licence](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/SmallChi/JT808DotNetty/blob/master/LICENSE)

## 新网关的优势：

1. 跨平台
2. 借助 .NET Core模块化的思想
3. 单机同时一万辆车在线不是梦(真有一万辆车那都很吃香了<(￣3￣)> <(￣3￣)> <(￣3￣)>  )
4. 简单易上手

## 设计模型

![design_model](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/img/design_model.png)

## 基于Tcp的消息业务处理程序（JT808.DotNetty.Tcp）

通过继承JT808.DotNetty.Core.Handlers.JT808MsgIdTcpHandlerBase去实现自定义的消息业务处理程序。

## 基于Udp的消息业务处理程序（JT808.DotNetty.Udp）

通过继承JT808.DotNetty.Core.Handlers.JT808MsgIdUdpHandlerBase去实现自定义的消息业务处理程序。

## 基于WebApi的消息业务处理程序（JT808.DotNetty.WebApi）

通过继承JT808.DotNetty.Core.Handlers.JT808MsgIdHttpHandlerBase去实现自定义的WebApi接口服务。

[WebApi公共接口服务](https://github.com/SmallChi/JT808DotNetty/blob/master/api/README.md)

## 集成接口功能（JT808.DotNetty.Abstractions）

|接口名称|接口说明|使用场景|
|:------:|:------|:------|
| IJT808SessionPublishing| 会话通知（在线/离线）| 有些超长待机的设备，不会实时保持连接，那么通过平台下发的命令是无法到达的，这时候就需要设备一上线，就即时通知服务去处理，然后在即时的下发消息到设备。|
| IJT808SourcePackageDispatcher| 原包分发器| 需要将源数据转给其他平台|

> 只要实现IJT808SessionPublishing接口的任意一款MQ都能实现该功能。

## NuGet安装

| Package Name          | Version                                            | Downloads                                           |
| --------------------- | -------------------------------------------------- | --------------------------------------------------- |
| Install-Package JT808.DotNetty.Core | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Core.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Core.svg) |
| Install-Package JT808.DotNetty.Abstractions | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Abstractions.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Abstractions.svg) |
| Install-Package JT808.DotNetty.Tcp | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Tcp.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Tcp.svg) |
| Install-Package JT808.DotNetty.Udp | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.Udp.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.Udp.svg) |
| Install-Package JT808.DotNetty.WebApi | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.WebApi.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApi.svg) |
| Install-Package JT808.DotNetty.WebApiClientTool | ![JT808](https://img.shields.io/nuget/v/JT808.DotNetty.WebApiClientTool.svg) | ![JT808](https://img.shields.io/nuget/dt/JT808.DotNetty.WebApiClientTool.svg) |

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
            services.AddJT808Core(hostContext.Configuration)
                    .AddJT808TcpHost()
                    .AddJT808UdpHost()
                    .AddJT808WebApiHost();
            // 自定义Tcp消息处理业务
            services.Replace(new ServiceDescriptor(typeof(JT808MsgIdTcpHandlerBase), typeof(JT808MsgIdTcpCustomHandler), ServiceLifetime.Singleton));
            // 自定义Udp消息处理业务
            services.Replace(new ServiceDescriptor(typeof(JT808MsgIdUdpHandlerBase), typeof(JT808MsgIdUdpCustomHandler), ServiceLifetime.Singleton));
            // 自定义会话通知（在线/离线）使用异步方式
            //services.Replace(new ServiceDescriptor(typeof(IJT808SessionPublishing), typeof(CustomJT808SessionPublishing), ServiceLifetime.Singleton));
            //  自定义原包转发 使用异步方式
            //services.Replace(new ServiceDescriptor(typeof(IJT808SourcePackageDispatcher), typeof(CustomJT808SourcePackageDispatcher), ServiceLifetime.Singleton));
            // webapi客户端调用
            services.AddHttpApi<IJT808DotNettyWebApi>().ConfigureHttpApiConfig((c, p) =>
            {
                c.HttpHost = new Uri("http://localhost:12828/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
                c.LoggerFactory = p.GetRequiredService<ILoggerFactory>();
            });
            var client = services.BuildServiceProvider().GetRequiredService<IJT808DotNettyWebApi>();
            var result = client.GetTcpAtomicCounter().InvokeAsync().Result;
        });

    await serverHostBuilder.RunConsoleAsync();
}
```

如图所示：
![demo1](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/img/demo1.png)
