# JT808 WebApi服务

基地址：<a href="#">http://localhost:828/jt808api/</a>

数据格式：只支持Json格式

默认端口：828

## [统一下发设备消息服务](#send)

## [管理会话服务](#session)

## [原包分发器通道服务](#sourcepackage)

## [转发地址过滤服务](#transmit)

## [消息包计数服务](#counter)

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

### <span id="send">统一下发设备消息接口</span>

请求地址：UnificationSend

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

### <span id="session">会话服务接口</span>

#### 统一会话信息对象返回 JT808SessionInfoDto

|属性|数据类型|参数说明|
|------|------|------|
| ChannelId| string| 通道Id|
| LastActiveTime| DateTime| 最后上线时间|
| StartTime| DateTime| 上线时间|
| TerminalPhoneNo|string| 终端手机号|
| RemoteAddressIP| string| 远程ip地址|

#### 1.获取会话集合

请求地址：Session/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| List\<JT808SessionInfoDto> | 实际会话信息集合 |

返回结果：

``` session1
{
    "Message":"",
    "Code":200,
    "Data":[
        {
            "ChannelId":"eadad23",
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789012",
            "RemoteAddressIP":"127.0.0.1:11808"
        },{
            "ChannelId":"eadad23",
            "LastActiveTime":"2018-11-27 20:00:00",
            "StartTime":"2018-11-25 20:00:00",
            "TerminalPhoneNo":"123456789013",
            "RemoteAddressIP":"127.0.0.1:11808"
        }
    ]
}
```

#### 2.通过设备终端号移除对应会话

请求地址：Session/RemoveByTerminalPhoneNo

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

### <span id="sourcepackage">原包分发器通道服务</span>

#### 1.添加原包转发地址

请求地址：SourcePackage/Add

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Host| string| ip地址|
| Port| int| 端口号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功

返回结果：

``` sp1
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

#### 2.删除原包转发地址（不能删除在网关服务器配置文件配的地址）

请求地址：SourcePackage/Remove

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Host| string| ip地址|
| Port| int| 端口号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功|

返回结果：

``` sp2
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

#### 3.获取原包信息集合

请求地址：SourcePackage/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|------|:------:|:------|
| RemoteAddress| string | 远程ip地址|
| Registered| bool | 通道是否注册|
| Active| bool | 通道是否激活|
| Open| bool | 通道是否打开|

返回结果：

``` sp3
{
    "Message":"",
    "Code":200,
    "Data":[
         {
            "RemoteAddress":"127.0.0.1:6665",
            "Registered":true,
            "Active":true,
            "Open":true
        },{
            "RemoteAddress":"127.0.0.1:6667",
            "Registered":true,
            "Active":true,
            "Open":true
        }
    ]
}
```

### <span id="transmit">转发地址过滤服务</span>

#### 1.添加转发过滤地址

请求地址：Transmit/Add

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Host| string| ip地址|
| Port| int| 端口号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功

返回结果：

``` tr1
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

#### 2.删除转发过滤地址（不能删除在网关服务器配置文件配的地址）

请求地址：Transmit/Remove

请求方式：POST

请求参数：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Host| string| ip地址|
| Port| int| 端口号|

返回数据：

|属性|数据类型|参数说明|
|:------:|:------:|:------|
| Data| bool | 是否成功|

返回结果：

``` tr2
{
    "Message":"",
    "Code":200,
    "Data":true
}
```

#### 3.获取转发过滤地址信息集合

请求地址：Transmit/GetAll

请求方式：GET

返回数据：

|属性|数据类型|参数说明|
|------|:------:|:------|
| Data| List\<string> | 远程ip地址|

返回结果：

``` tr3
{
    "Message":"",
    "Code":200,
    "Data":[
        "127.0.0.1:6665",
        "127.0.0.1:6667"
    ]
}
```

### <span id="counter">计数服务接口</span>

请求地址：GetAtomicCounter

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