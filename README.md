# JT808DotNetty

基于DotNetty封装的JT808DotNetty通用消息业务处理

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

## 集成功能实现

### 1.集成原包分发器

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| ISourcePackageDispatcher | 原包分发器(支持热更新、断线重连) | 需要将原数据转给其他平台 |

### 2.集成WebApi服务器

[WebApi接口服务](https://github.com/SmallChi/JT808DotNetty/blob/master/api/README.md)

### 3.集成业务消息处理程序

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| JT808MsgIdHandlerBase | 业务消息处理程序 | 需要自定义实现业务消息处理程序 |

### 举个栗子1

#### 3.1.实现业务消息处理程序JT808MsgIdHandlerBase

```business Imp
public class JT808MsgIdCustomHandler : JT808MsgIdHandlerBase
{
    private readonly ILogger<JT808MsgIdCustomHandler> logger;

    public JT808MsgIdCustomHandler(ILoggerFactory loggerFactory,
        JT808SessionManager sessionManager) : base(sessionManager)
    {
        logger = loggerFactory.CreateLogger<JT808MsgIdCustomHandler>();
    }

    public override JT808Response Msg0x0102(JT808Request request)
    {
        logger.LogDebug("Msg0x0102");
        return base.Msg0x0102(request);
    }
}

```

#### 3.2.自定义业务消息处理程序替换默认实现

``` handler
services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHandlerBase), typeof(JT808MsgIdCustomHandler), ServiceLifetime.Singleton));
```

#### 3.3.使用JT808 Host

``` host
  UseJT808Host()
```

#### 3.4.完整示例

``` demo
// 默认网关端口：808
// 默认webapi端口：828
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
            logging.SetMinimumLevel(LogLevel.Error);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHandlerBase), typeof(JT808MsgIdCustomHandler), ServiceLifetime.Singleton));
        })
        .UseJT808Host();
    await serverHostBuilder.RunConsoleAsync();
}
```