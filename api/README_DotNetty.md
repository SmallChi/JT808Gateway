# 基于JT808DotNetty WebApi 接口文档

基地址：127.0.0.1:828/jt808api/

> 注意url格式

数据格式：只支持Json格式

默认端口：828

## 1.统一下发设备消息服务

[统一下发设备消息服务](#send)

## 2.管理会话服务

[基于Tcp管理会话服务](#tcp_session)

[基于Udp管理会话服务](#udp_session)

## 4.消息包计数服务

[基于Tcp消息包计数服务](#tcp_counter)

[基于Udp消息包计数服务](#udp_counter)

## 接口请求对照表

### 公共接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/UnificationSend| POST| 统一下发设备消息服务|

### 基于Tcp接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/Tcp/Session/GetAll| GET| 基于Tcp管理会话服务-获取会话集合|
| 127.0.0.1:828/jt808api/Tcp/Session/QueryTcpSessionByTerminalPhoneNo| POST| 基于Tcp管理会话服务-通过设备终端号查询对应会话|
| 127.0.0.1:828/jt808api/Tcp/Session/RemoveByTerminalPhoneNo| POST| 基于Tcp管理会话服务-通过设备终端号移除对应会话|
| 127.0.0.1:828/jt808api/Tcp/GetAtomicCounter| GET| 基于Tcp消息包计数服务|

### 基于Udp接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/Udp/Session/GetAll| GET| 基于Udp管理会话服务-获取会话集合|
| 127.0.0.1:828/jt808api/Udp/Session/RemoveByTerminalPhoneNo| POST| 基于Udp管理会话服务-通过设备终端号移除对应会话|
| 127.0.0.1:828/jt808api/Udp/GetAtomicCounter| GET| 基于Udp消息包计数服务|

### 统一对象返回 JT808ResultDto\<T>

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

### <span id="send">基于Tcp统一下发设备消息服务</span>

请求地址：/UnificationSend

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|------|:------:|:------|
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

### <span id="tcp_session">基于Tcp管理会话服务</span>

#### 统一会话信息对象返回 JT808TcpSessionInfoDto

|属性|数据类型|参数说明|
|------|------|------|
| LastActiveTime| DateTime| 最后上线时间|
| StartTime| DateTime| 上线时间|
| TerminalPhoneNo|string| 终端手机号|
| RemoteAddressIP| string| 远程ip地址|

#### 1.获取会话集合

请求地址：Tcp/Session/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808TcpSessionInfoDto> | 实际会话信息集合 |

返回结果：

``` session1
{
    "Message":"",
    "Code":200,
    "Data":[
        {
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789012",
            "RemoteAddressIP":"127.0.0.1:11808"
        },{
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789013",
            "RemoteAddressIP":"127.0.0.1:11808"
        }
    ]
}
```

#### 2.通过设备终端号移除对应会话

请求地址：Tcp/Session/RemoveByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| terminalPhoneNo| string| 设备终端号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功

返回结果：

``` session3
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

### <span id="udp_session">基于Udp管理会话服务</span>

#### 统一会话信息对象返回 JT808UdpSessionInfoDto

|属性|数据类型|参数说明|
|------|------|------|
| LastActiveTime| DateTime| 最后上线时间|
| StartTime| DateTime| 上线时间|
| TerminalPhoneNo|string| 终端手机号|
| RemoteAddressIP| string| 远程ip地址|

#### 1.获取会话集合

请求地址：Udp/Session/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808UdpSessionInfoDto> | 实际会话信息集合 |

返回结果：

``` session1
{
    "Message":"",
    "Code":200,
    "Data":[
        {
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789012",
            "RemoteAddressIP":"127.0.0.1:11808"
        },{
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789013",
            "RemoteAddressIP":"127.0.0.1:11808"
        }
    ]
}
```

#### 2.通过设备终端号移除对应会话

请求地址：Udp/Session/RemoveByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| terminalPhoneNo| string| 设备终端号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功

返回结果：

``` session3
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

### <span id="tcp_counter">基于Tcp消息包计数服务</span>

请求地址：Tcp/GetAtomicCounter

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|------|:------:|:------|
| MsgSuccessCount| long| 消息包成功数|
| MsgFailCount| long| 消息包失败数|

返回结果：

``` counter
{
    "Message":"",
    "Code":200,
    "Data":{
        "MsgSuccessCount":10000,
        "MsgFailCount":0
    }
}
```

### <span id="udp_counter">基于Udp消息包计数服务</span>

请求地址：Udp/GetAtomicCounter

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|------|:------:|:------|
| MsgSuccessCount| long| 消息包成功数|
| MsgFailCount| long| 消息包失败数|

返回结果：

``` counter
{
    "Message":"",
    "Code":200,
    "Data":{
        "MsgSuccessCount":1000,
        "MsgFailCount":0
    }
}
```
