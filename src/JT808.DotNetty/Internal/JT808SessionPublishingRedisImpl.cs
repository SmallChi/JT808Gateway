using JT808.DotNetty.Interfaces;
using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using JT808.DotNetty.Configurations;

namespace JT808.DotNetty.Internal
{
    internal class JT808SessionPublishingRedisImpl : IJT808SessionPublishing,IDisposable
    {
        private IConnectionMultiplexer connectionMultiplexer;

        private IOptionsMonitor<JT808Configuration> optionsMonitor;

        private string redisHost;

        private IDisposable optionsMonitorDisposable;

        public JT808SessionPublishingRedisImpl(
            IOptionsMonitor<JT808Configuration> optionsMonitor
            )
        {
            this.optionsMonitor = optionsMonitor;
            redisHost = optionsMonitor.CurrentValue.RedisHost;
            try
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(redisHost);
            }
            catch
            {

            }
            optionsMonitorDisposable= this.optionsMonitor.OnChange((config,str) => 
            {
                if(config.RedisHost!= redisHost)
                {
                    redisHost = config.RedisHost;
                    connectionMultiplexer.Close();
                    try
                    {
                        connectionMultiplexer = ConnectionMultiplexer.Connect(redisHost);
                    }
                    catch
                    {
                        
                    }
                }
            });
        }

        public Task PublishAsync(string topicName, string key, string value)
        {
            if (connectionMultiplexer.IsConnected)
            {
                Subscriber?.PublishAsync(topicName, value);
            }
            return Task.CompletedTask;
        }

        internal ISubscriber Subscriber
        {
            get
            {
                return connectionMultiplexer.GetSubscriber();
            }
        }

        public void Dispose()
        {
            connectionMultiplexer.Close();
            optionsMonitorDisposable.Dispose();
        }
    }
}
