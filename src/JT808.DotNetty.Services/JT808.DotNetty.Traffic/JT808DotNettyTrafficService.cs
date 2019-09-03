using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Traffic
{
    public class JT808DotNettyTrafficService:IDisposable
    {
        private readonly CSRedis.CSRedisClient redisClien;
        public JT808DotNettyTrafficService(IConfiguration configuration)
        {
            redisClien = new CSRedis.CSRedisClient(configuration.GetConnectionString("TrafficRedisHost"));
            TrafficRedisClient.Initialization(redisClien);
        }

        public void Dispose()
        {
            redisClien.Dispose();
        }

        /// <summary>
        /// 按设备每天统计sim卡流量
        /// </summary>
        /// <param name="terminalNo"></param>
        /// <param name="len"></param>
        public void Processor(string terminalNo,int len)
        {
            TrafficRedisClient.HIncrBy(terminalNo, DateTime.Now.ToString("yyyyMMdd"), len);
        }
    }
}
