# JT808DotNetty

基于DotNetty封装的JT808DotNetty专注消息业务处理

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

## NuGet安装

| Package Name          | Version                                            | Downloads                                           |
| --------------------- | -------------------------------------------------- | --------------------------------------------------- |
| Install-Package JT808DotNetty | ![JT808DotNetty](https://img.shields.io/nuget/v/JT808DotNetty.svg) | ![JT808DotNetty](https://img.shields.io/nuget/dt/JT808DotNetty.svg) |

## 集成功能实现

### 1.集成原包分发器

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| ISourcePackageDispatcher | 原包分发器(支持热更新、断线重连) | 需要将原数据转给其他平台 |

### 2.集成WebApi服务器

#### [2.1.统一下发设备消息服务 IJT808UnificationSendService](#send)

#### [2.2.管理会话服务 IJT808SessionService](#session)

#### [2.3.消息包计数服务 JT808AtomicCounterService 接口尚未实现](#counter)

### 3.集成业务消息处理程序

| 功能 | 说明 | 使用场景 |
|:-------:|:-------:|:-------:|
| JT808MsgIdHandlerBase | 业务消息处理程序 | 需要自定义实现业务消息处理程序 |

### 举个栗子1

#### 1.实现业务消息处理程序JT808MsgIdHandlerBase

```business Impl
using JT808.DotNetty;
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
#### 2.自定义业务消息处理程序替换默认实现

```
services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHandlerBase), typeof(JT808MsgIdCustomHandler), ServiceLifetime.Singleton));
```
#### 3.使用JT808 Host

``` host
  UseJT808Host()
```

#### 4.完整示例
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

## 提供WebApi接口服务（默认端口828）

基地址：<a href="#">http://localhost:828/jt808api/</a>
数据格式：只支持Json格式

#### 统一对象返回 JT808ResultDto\<T>

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Message| string| 消息描述|
| Code| int| 状态码|
| Data| T（泛型）| 数据|

返回Code[状态码]说明：
|状态码|说明|
|:------:|:------:|
| 200 | 返回成功 |
| 201 | 内容为空 |
| 404 | 没有该服务 |
| 500 | 服务内部错误 |

#### <span id="send">统一下发设备消息接口</span>

请求地址：UnificationSend
请求方式：POST
请求参数：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|
| Data| byte[]| JT808 byte[]数组|
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool| 是否成功|
返回结果：
``` result1
{
    "Message":"",
    "Code":200,
    "Data":true
}
```
#### <span id="session">会话服务接口</span>

##### 统一会话信息对象返回 JT808SessionInfoDto

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| ChannelId| string| 通道Id|
| LastActiveTime| DateTime| 最后上线时间|
| StartTime| DateTime| 上线时间|
| TerminalPhoneNo|string| 终端手机号|

##### 1.获取实际连接数(存在其他平台转发过来的数据，这时候通道Id和设备属于一对多的关系)

请求地址：Session/GetRealLinkCount
请求方式：GET
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| int| 实际连接数|
返回结果：
``` result1
{
    "Message":"",
    "Code":200,
    "Data":10
}
```
##### 2.获取设备相关连的连接数

请求地址：Session/GetRelevanceLinkCount
请求方式：GET
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| int | 设备相关连的连接数 |
返回结果：
``` result2
{
    "Message":"",
    "Code":200,
    "Data":10
}
```
##### 3.获取实际会话集合

请求地址：Session/GetRealAll
请求方式：GET
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808SessionInfoDto> | 实际会话信息集合 |
返回结果：
``` result3
{
    "Message":"",
    "Code":200,
    "Data":[
        {
            "ChannelId":"eadad23",
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789012"
        },{
            "ChannelId":"eadad23",
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789013"
        }
    ]
}
```
##### 4.获取设备相关联会话集合

请求地址：Session/GetRelevanceAll
请求方式：GET
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808SessionInfoDto> | 设备相关联会话信息集合 |
返回结果：
``` result4
{
    "Message":"",
    "Code":200,
    "Data":[
        {
            "ChannelId":"eadad23",
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789012"
        }, {
            "ChannelId":"eadad24",
            "LastActiveTime":"2018-11-26 20:00:00",
            "StartTime":"2018-11-22 20:00:00",
            "TerminalPhoneNo":"123456789013"
        }
    ]
}
```

##### 5.通过通道Id移除对应会话

请求地址：Session/RemoveByChannelId
请求方式：POST
请求参数：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| channelId| string| 通道Id|
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功 |
返回结果：
``` result5
{
    "Message":"",
    "Code":200,
    "Data":true
}
```
##### 6.通过设备终端号移除对应会话

请求地址：Session/RemoveByTerminalPhoneNo
请求方式：POST
请求参数：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| terminalPhoneNo| string| 设备终端号|
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功 |
返回结果：
``` result6
{
    "Message":"",
    "Code":200,
    "Data":true
}
```
##### 7.通过通道Id获取会话信息

请求地址：Session/GetByChannelId
请求方式：POST
请求参数：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| channelId| string| 通道Id|
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| JT808SessionInfoDto | 会话信息对象 |
返回结果：
``` result7
{
    "Message":"",
    "Code":200,
    "Data":{
        "ChannelId":"eadad24",
        "LastActiveTime":"2018-11-26 20:00:00",
        "StartTime":"2018-11-22 20:00:00",
        "TerminalPhoneNo":"123456789013"
    }
}
```

##### 8.通过设备终端号获取会话信息

请求地址：Session/GetByTerminalPhoneNo
请求方式：POST
请求参数：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| terminalPhoneNo| string| 设备终端号|
返回数据：
|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| JT808SessionInfoDto | 会话信息对象 |
返回结果：
``` result8
{
    "Message":"",
    "Code":200,
    "Data":{
        "ChannelId":"eadad24",
        "LastActiveTime":"2018-11-26 20:00:00",
        "StartTime":"2018-11-22 20:00:00",
        "TerminalPhoneNo":"123456789013"
    }
}
```