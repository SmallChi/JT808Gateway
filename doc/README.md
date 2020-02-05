# 压力测试

## 基于pipeline

**只是谁便测试玩玩，反正机器便宜。**

> 注意1：连接数和并发数要区分开；
> 注意2：阿里云的机器默认有连接数限制(5000)，可以先创建一台，把该装的软件安装好，tcp参数内核调优后，在备份一个系统镜像在玩;
> 注意3: 使用的是网关集成的方式进行测试。

``` 1
//使用PM2托管

//服务端
cd /data/JT808.Gateway
pm2 start "dotnet JT808.Gateway.ServerBenchmark.dll ASPNETCORE_ENVIRONMENT=Production" --max-restarts=1 -n "JT808.Gateway.808" -o "/data/pm2Logs/JT808.Gateway/out.log" -e "/data/pm2Logs/JT808.Gateway/error.log"

//客户端
cd /data/JT808Client
pm2 start "dotnet JT808.Gateway.CleintBenchmark.dll ASPNETCORE_ENVIRONMENT=Production" --max-restarts=1 -n "JT808.Gateway.CleintBenchmark" -o "/data/pm2Logs/JT808.Gateway.CleintBenchmark/out.log" -e "/data/pm2Logs/JT808.Gateway.CleintBenchmark/error.log"

//可选的
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
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
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
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
  }
```

![server_proccess_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_20k.png)

![server_network_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_20k.png)

![client_20k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_20k.png)

### 30K

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
    "DeviceCount": 30000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
  }
```

![server_proccess_30k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_30k.png)

![server_network_30k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_30k.png)

![client_30k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_30k.png)

### 40K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端1参数配置appsettings.json

``` 1
  "urls": "http://*:15004;",
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 20000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

``` 2
  "urls": "http://*:15004;",
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 20000,
    "Interval": 1000,
    "DeviceTemplate": 200000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

> 一个进程的线程是有限的，所以分两个进程进行测试

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
  }
```

![server_proccess_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_40k.png)

![server_network_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_40k.png)

![client_40k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_40k.png)

### 60K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 8c16g | JT808服务端 |
| centos7 | 8c16g | JT808客户端1 |
| centos7 | 8c16g | JT808客户端2 |

> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端1参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 30000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

客户端2参数配置appsettings.json

``` 2
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 30000,
    "Interval": 1000,
    "DeviceTemplate": 200000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
  }
```

![server_proccess_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_60k.png)

![server_network_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_60k.png)

![client_60k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/client_60k.png)

### 100K

| 操作系统 | 配置 | 使用 |
|:-------:|:-------:|:-------:|
| centos7 | 16c24g | JT808服务端 |
| centos7 | 8c16g | JT808客户端 |
| centos7 | 8c16g | JT808客户端 |
| centos7 | 8c16g | JT808客户端 |
| centos7 | 8c16g | JT808客户端 |

> 计算网络增强型 sn1ne ecs.sn1ne.3xlarge 12 vCPU 24 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2.5 Gbps 130 万 PPS
> 计算网络增强型 sn1ne ecs.sn1ne.2xlarge 8 vCPU 16 GiB Intel Xeon E5-2682v4 / Intel Xeon(Skylake) Platinum 8163 2.5 GHz 2 Gbps 100 万 PPS

客户端1的参数配置appsettings.json

``` 1
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 25000,
    "Interval": 1000,
    "DeviceTemplate": 100000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

客户端2的参数配置appsettings.json

``` 2
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 25000,
    "Interval": 1000,
    "DeviceTemplate": 200000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

客户端3的参数配置appsettings.json

``` 3
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 25000,
    "Interval": 1000,
    "DeviceTemplate": 300000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

客户端3的参数配置appsettings.json

``` 4
  "ClientBenchmarkOptions": {
    "IP": "",
    "Port": 808,
    "DeviceCount": 25000,
    "Interval": 1000,
    "DeviceTemplate": 400000 //需要多台机器同时访问，那么可以根据这个避开重复终端号 100000-200000-300000
  }
```

服务器参数配置appsettings.json

``` 1
  "JT808Configuration": {
    "TcpPort": 808,
    "UdpPort": 808,
    "MiniNumBufferSize": 102400,
    "SoBacklog": 204800
  }
```

![server_proccess_100k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_proccess_100k.png)

![server_network_100k](https://github.com/SmallChi/JT808Gateway/blob/master/doc/pipeline/server_network_100k.png)

### 参考Centos7内核参数调优

``` 99
vi /etc/security/limits.conf

修改如下：
root soft nofile 204800
root hard nofile 204800
* soft nofile 204800
* hard nofile 204800

vi /etc/pam.d/login
添加如下：
session    required     pam_limits.so

vi /etc/sysctl.conf
添加如下：
vm.swappiness = 0
net.ipv4.neigh.default.gc_stale_time = 120
# see details in https://help.aliyun.com/knowledge_detail/39428.html
net.ipv4.conf.all.rp_filter = 0
net.ipv4.conf.default.rp_filter = 0
net.ipv4.conf.default.arp_announce = 2
net.ipv4.conf.lo.arp_announce = 2
net.ipv4.conf.all.arp_announce = 2
# see details in https://help.aliyun.com/knowledge_detail/41334.html
net.ipv4.tcp_synack_retries = 2
net.ipv4.tcp_syn_retries = 1
net.ipv4.tcp_synack_retries = 1
net.ipv4.tcp_keepalive_time = 600
net.ipv4.tcp_keepalive_probes = 3
net.ipv4.tcp_keepalive_intvl =15
net.ipv4.tcp_retries2 = 5
net.ipv4.tcp_fin_timeout = 1
net.ipv4.tcp_max_tw_buckets = 65535
net.ipv4.tcp_tw_recycle = 1
net.ipv4.tcp_tw_reuse = 1
net.ipv4.tcp_max_orphans = 32768
net.ipv4.tcp_syncookies = 1
net.ipv4.tcp_max_syn_backlog = 65535
net.ipv4.tcp_wmem = 8192 131072 16777216
net.ipv4.tcp_rmem = 32768 131072 16777216
net.ipv4.tcp_mem = 94500000 915000000 927000000
net.ipv4.ip_local_port_range = 1024 65000
net.core.somaxconn = 65535
net.core.netdev_max_backlog = 65535
fs.file-max = 265535
net.ipv6.conf.lo.disable_ipv6 = 1
kernel.sysrq = 1
net.ipv6.conf.all.disable_ipv6 = 1
net.ipv6.conf.default.disable_ipv6 = 1

最后重启机器下：
reboot
```

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
