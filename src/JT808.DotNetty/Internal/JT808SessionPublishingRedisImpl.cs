using JT808.DotNetty.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using JT808.DotNetty.Configurations;
using Microsoft.Extensions.Logging;

namespace JT808.DotNetty.Internal
{
    internal class JT808SessionPublishingRedisImpl : IJT808SessionPublishing
    {
        private IConnectionMultiplexer connectionMultiplexer;

        private IOptionsMonitor<JT808Configuration> optionsMonitor;

        private ILogger<JT808SessionPublishingRedisImpl> logger;

        private JT808SessionPublishingRedisImpl(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<JT808Configuration> optionsMonitor
            )
        {
            this.optionsMonitor = optionsMonitor;
            logger = loggerFactory.CreateLogger<JT808SessionPublishingRedisImpl>();
            connectionMultiplexer = ConnectionMultiplexer.Connect(optionsMonitor.CurrentValue.RedisHost);
        }

        public Task PublishAsync(string topicName, string key, string value)
        {
            if (connectionMultiplexer.IsConnected)
            {

            }
            return Task.CompletedTask;
        }
    }
}
