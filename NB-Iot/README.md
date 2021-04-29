# 中国电信物联网开发平台

这几天对接NB-Iot的设备发现了既然还支持JT/T808的，索性就抱着玩玩的心态搞一搞。

[中国电信物联网开放平台AEP](https://www.ctwing.cn/)

[中国电信官方JT/T808协议规范参考文档](https://help.ctwing.cn/%E8%AE%BE%E5%A4%87%E6%8E%A5%E5%85%A5%E8%A7%84%E8%8C%83/jtt808%E5%8D%8F%E8%AE%AE%E6%8E%A5%E5%85%A5/jtt808%E5%8D%8F%E8%AE%AE%E8%A7%84%E8%8C%83.html)

## 使用教程

> 前提条件：需要注册和实名登记之后才可以使用电信平台

1.开通终端接入和MQ消息推送服务

2.再设备管理->产品，创建对应的JT/T808产品，如图所示

![创建产品](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/product.png)

3.再对应的产品中按图所示进行点击进入设备添加列表

![设备1](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/device1.png)

![设备2](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/device2.png)

4.再MQ消息推送里面创建对应的Topic，如图所示

![mq](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/mq.png)

> 注意：由于电信平台只提供java版本的sdk，这边需要去下载对应的语言包的sdk[中国电信使用的MQ开源库](http://pulsar.apache.org/docs/en/client-libraries-dotnet/#installation)

5.模拟设备上电信平台

需要注意几项：

5.1.这边设备模拟的是2013版本的808协议，平台给的参考是2019版本，但是例子里面确是2013版本的；

5.2.模拟的终端注册的关键三个参数，制造商编号、设备型号、设备编号这三个参数很重要，很重要，很重要。

6.设备与消息进行调试如图所示

![debug1](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/debug1.png)

![debug2](https://github.com/SmallChi/JT808Gateway/blob/master/NB-Iot/img/debug2.png)

7.使用JT808.Gateway.NBIotSimpleClient项目进行模拟测试

需要修改一下参数配置：

7.1.Jobs目录下面的Up2013Service文件

```1
MakerId = "12345",         //制造商编号
TerminalModel = "123456",  //设备型号
TerminalId = "1234567",    //设备编号
```

7.2.Services目录下面的AEPMsgConsumerService文件

```2
//自己填写电信平台配置的参数
 string topic = "test";
//自己填写电信平台配置的参数
string tenantId = "";
//自己填写电信平台配置的参数
string token = "";
```

7.3.以上配置好就可以运行看效果了

> 要是不成功的话，那多半以上没有配置好，导致的。
