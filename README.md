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

## 集成功能实现

### 1.集成原包分发器

### 2.集成WebApi服务器

[WebApi接口服务](https://github.com/SmallChi/JT808DotNetty/blob/master/api/README.md)

### 3.集成会话通知（在线/离线）

使用场景：有些超长待机的设备，不会实时保持连接，那么通过平台下发的命令是无法到达的，这时候就需要设备一上线，就即时通知服务去处理，然后在即时的下发消息到设备。

> 只要实现IJT808SessionPublishing接口的任意一款MQ都能实现该功能。

### 4.集成业务消息处理程序