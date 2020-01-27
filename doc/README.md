# 压力测试

## 基于DotNetty

[感谢泥水佬提供的压力测试工具](https://www.cnblogs.com/smark/p/4496660.html?utm_source=tuicool)

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| win server 2016 | 4c8g | 压力测试客户端 |
| centos7 | 4c8g | JT808服务端 |

![performance_1000](https://github.com/SmallChi/JT808Gateway/blob/master/doc/dotnetty/performance_1000.png)

![performance_2000](https://github.com/SmallChi/JT808Gateway/blob/master/doc/dotnetty/performance_2000.png)

![performance_5000](https://github.com/SmallChi/JT808Gateway/blob/master/doc/dotnetty/performance_5000.png)

![performance_10000](https://github.com/SmallChi/JT808Gateway/blob/master/doc/dotnetty/performance_10000.png)

## 基于pipeline

**由于【参数配置】不同导致测试的效果可能不同，只是谁便测试玩玩，反正机器便宜。**

> 注意1：连接数和并发数要区分开；
> 注意2：阿里云的机器默认有连接数限制(5000)，可以先创建一台，把该装的软件安装好，tcp参数内核调优后，在备份一个系统镜像在玩。

``` 1
使用PM2托管

//服务端
cd /data/JT808.Gateway
pm2 start "dotnet JT808.Gateway.TestHosting.dll ASPNETCORE_ENVIRONMENT=Production" --max-restarts=1 -n "JT808.Gateway.808" -o "/data/pm2Logs/JT808.Gateway/out.log" -e "/data/pm2Logs/JT808.Gateway/error.log"

//客户端
cd /data/JT808Client
pm2 start "dotnet JT808.Gateway.CleintBenchmark.dll ASPNETCORE_ENVIRONMENT=Production" --max-restarts=1 -n "JT808.Gateway.CleintBenchmark" -o "/data/pm2Logs/JT808.Gateway.CleintBenchmark/out.log" -e "/data/pm2Logs/JT808.Gateway.CleintBenchmark/error.log"

修改wwwroot下index.html的webapi接口地址
127.0.0.1:15004/index.html
```

### 10K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 10000,
    "Interval": 10,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

![server_proccess_10k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_10k.png)

![server_network_10k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_10k.png)

![client_10k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_10k.png)

### 20K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 20000,
    "Interval": 300,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

![server_proccess_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_20k.png)

![server_network_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_20k.png)

![client_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_20k.png)

### 40K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 40000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

![server_proccess_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_40k.png)

![server_network_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_40k.png)

![client_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_40k.png)

### 60K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 60000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

![server_proccess_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_60k.png)

![server_network_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_60k.png)

![client_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_60k.png)

### 80K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端1的参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 40000,
    "Interval": 3000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

客户端2的参数配置appsettings.json

``` 2
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 40000,
    "Interval": 3000,
    "DeviceTemplate": 200000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

![server_proccess_80k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_80k.png)

![server_network_80k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_80k.png)

![client_80k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_80k.png)