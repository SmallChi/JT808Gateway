# 压力测试

## 基于DotNetty

[感谢泥水佬提供的压力测试工具](https://www.cnblogs.com/smark/p/4496660.html?utm_source=tuicool)

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| win server 2016 | 4c8g | 压力测试客户端 |
| centos7 | 4c8g | JT808服务端 |

![performance_1000](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/performance_1000.png)

![performance_2000](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/performance_2000.png)

![performance_5000](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/performance_5000.png)

![performance_10000](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/dotnetty/performance_10000.png)

## 基于pipeline

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 4c8g | JT808服务端 |
| centos7 | 4c8g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.xlarge 4 vCPU 8 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 1.5 Gbps 50 万 PPS

![server_proccess_10k](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/pipeline/server_proccess_10k.png)

![server_network_10k](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/pipeline/server_network_10k.png)

![client_10k](https://github.com/SmallChi/JT808DotNetty/blob/master/doc/pipeline/client_10k.png)
