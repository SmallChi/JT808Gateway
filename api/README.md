# 基于JT808Gateway WebApi 接口文档

基地址：127.0.0.1:828/jt808api/

> 注意url格式

数据格式：只支持Json格式

默认端口：828

## 1.统一下发设备消息服务

[统一下发设备消息服务](#send)

## 2.管理会话服务

[基于Tcp管理会话服务](#tcp_session)

[基于Udp管理会话服务](#udp_session)

## 3.SIM黑名单管理服务

[SIM黑名单管理服务](#blacklist)

## 接口请求对照表

### 公共接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/UnificationSend| POST| 统一下发设备消息服务|

### 基于Tcp接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/Tcp/Session/GetAll| GET| 基于Tcp管理会话服务-获取会话集合|
| 127.0.0.1:828/jt808api/Tcp/Session/SessionTcpByPage?pageIndex=0&pageSize10| GET| 基于Tcp管理会话服务-获取会话分页集合|
| 127.0.0.1:828/jt808api/Tcp/Session/QuerySessionByTerminalPhoneNo| POST| 基于Tcp管理会话服务-通过设备终端号查询对应会话|
| 127.0.0.1:828/jt808api/Tcp/Session/RemoveByTerminalPhoneNo| POST| 基于Tcp管理会话服务-通过设备终端号移除对应会话|

### 基于Udp接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/Udp/Session/GetAll| GET| 基于Udp管理会话服务-获取会话集合|
| 127.0.0.1:828/jt808api/Udp/Session/SessionUdpByPage?pageIndex=0&pageSize10| GET| 基于Tcp管理会话服务-获取会话分页集合|
| 127.0.0.1:828/jt808api/Udp/Session/QuerySessionByTerminalPhoneNo| POST| 基于Udp管理会话服务-通过设备终端号查询对应会话|
| 127.0.0.1:828/jt808api/Udp/Session/RemoveByTerminalPhoneNo| POST| 基于Udp管理会话服务-通过设备终端号移除对应会话|

### SIM黑名单管理接口请求

|请求Url|请求方式|说明|
|:------|:------|:------|
| 127.0.0.1:828/jt808api/Blacklist/Add| POST| SIM卡黑名单服务-将对应SIM号加入黑名单|
| 127.0.0.1:828/jt808api/Blacklist/Remove| POST| SIM卡黑名单服务-将对应SIM号移除黑名单|
| 127.0.0.1:828/jt808api/Blacklist/GetAll| Get| SIM卡黑名单服务-获取所有sim的黑名单列表|

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
| HexData| string| JT808 Hex String JT808数据包字符串|

``` 1
{
    "TerminalPhoneNo":"123456789012",
    "HexData":"7E****7E"
}
```

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

#### 2.通过设备终端号查询对应会话

请求地址：Tcp/Session/QuerySessionByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| JT808TcpSessionInfoDto对象 | 统一会话信息对象返回 |

返回结果：

``` session2
{
    "Message":"",
    "Code":200,
    "Data": {
        "LastActiveTime":"2018-11-27 20:00:00",
        "StartTime":"2018-11-25 20:00:00",
        "TerminalPhoneNo":"123456789012",
        "RemoteAddressIP":"127.0.0.1:11808"
    }
}
```

#### 3.通过设备终端号移除对应会话

请求地址：Tcp/Session/RemoveByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

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

#### 4.获取会话分页集合

请求地址：Tcp/Session/SessionTcpByPage

请求方式：GET

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| pageIndex| int| 当前页（默认0）|
| pageSize| int| 页容量（默认10）|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808TcpSessionInfoDto> | 实际会话信息集合 |
| PageIndex| int | 当前页（默认0） |
| PageSize| int | 页容量（默认10） |
| Total| int | 总数 |

返回结果：

``` session1
{
    "message":null,
    "code":200,
    "data":{
        "pageIndex":0,
        "pageSize":10,
        "total":2,
        "data":[
            {
                "lastActiveTime":"2022-09-03T19:34:07.8733605+08:00",
                "startTime":"2022-09-03T19:34:07.8733615+08:00",
                "terminalPhoneNo":"123456789012",
                "remoteAddressIP":"127.0.0.1:9826"
            },
            {
                "lastActiveTime":"2022-09-03T19:34:05.135997+08:00",
                "startTime":"2022-09-03T19:34:05.136035+08:00",
                "terminalPhoneNo":"123456789013",
                "remoteAddressIP":"127.0.0.1:9825"
            }
        ]
    }
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

#### 2.通过设备终端号查询对应会话

请求地址：Udp/Session/QuerySessionByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| JT808UdpSessionInfoDto对象 | 统一会话信息对象返回 |

返回结果：

``` session2
{
    "Message":"",
    "Code":200,
    "Data":{
        "LastActiveTime":"2018-11-27 20:00:00",
        "StartTime":"2018-11-25 20:00:00",
        "TerminalPhoneNo":"123456789012",
        "RemoteAddressIP":"127.0.0.1:11808"
    }
}
```

#### 3.通过设备终端号移除对应会话

请求地址：Udp/Session/RemoveByTerminalPhoneNo

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

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

#### 4.获取会话分页集合

请求地址：Udp/Session/SessionUdpByPage

请求方式：GET

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| pageIndex| int| 当前页（默认0）|
| pageSize| int| 页容量（默认10）|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808UdpSessionInfoDto> | 实际会话信息集合 |
| PageIndex| int | 当前页（默认0） |
| PageSize| int | 页容量（默认10） |
| Total| int | 总数 |

返回结果：

``` session1
{
    "message":null,
    "code":200,
    "data":{
        "pageIndex":0,
        "pageSize":10,
        "total":2,
        "data":[
            {
                "lastActiveTime":"2022-09-03T19:34:07.8733605+08:00",
                "startTime":"2022-09-03T19:34:07.8733615+08:00",
                "terminalPhoneNo":"123456789012",
                "remoteAddressIP":"127.0.0.1:9826"
            },
            {
                "lastActiveTime":"2022-09-03T19:34:05.135997+08:00",
                "startTime":"2022-09-03T19:34:05.136035+08:00",
                "terminalPhoneNo":"123456789013",
                "remoteAddressIP":"127.0.0.1:9825"
            }
        ]
    }
}
```

### <span id="blacklist">SIM黑名单管理服务</span>

#### 1.添加sim卡黑名单

请求地址：Blacklist/Add

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

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

#### 2.移除sim卡黑名单

请求地址：Blacklist/Remove

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| TerminalPhoneNo| string| 设备终端号|

``` 1
{
    "TerminalPhoneNo":"123456789012",
}
```

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

#### 3.获取sim卡黑名单

请求地址：Blacklist/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| terminalPhoneNo| List\<string>| 设备终端号集合|

返回结果：

``` session3
{
    "Message":"",
    "Code":200,
    "Data":[
        "12345678901",
        "12345678902"
    ]
}
```
