using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Kafka;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Traffic.Test
{
    public class JT808DotNettyTrafficServiceTest
    {
        private readonly CSRedis.CSRedisClient redisClien;
        public readonly ILogger<JT808DotNettyTrafficServiceTest> logger;
        public JT808DotNettyTrafficServiceTest(ILoggerFactory loggerFactory) {
            redisClien = new CSRedis.CSRedisClient("127.0.0.1:6379,password=smallchi");
            RedisHelper.Initialization(redisClien);
            logger = loggerFactory.CreateLogger<JT808DotNettyTrafficServiceTest>();
            Task.Run(() => {
                while (true)
                {
                    Thread.Sleep(5000);
                    using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(new JT808MsgProducerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        TopicName = "JT808Msg"
                    }))
                    {
                        jT808MsgProducer.ProduceAsync("011111111111", "7E02000032011111111111012E00000000000C00000160E42506C30C82002C00000000180914142057010400001DC003020000250400000000300115310100977E".ToHexBytes()).Wait();
                    }
                   var length= RedisHelper.HGet("011111111111", DateTime.Now.ToString("yyyyMMdd"));
                    logger.LogDebug($"{011111111111}:{length}");
                }
            });
        }
    }
}
