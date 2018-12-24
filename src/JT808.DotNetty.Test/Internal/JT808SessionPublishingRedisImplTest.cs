using JT808.DotNetty.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using JT808.DotNetty.Configurations;
using Microsoft.Extensions.Options;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using StackExchange.Redis;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808SessionPublishingRedisImplTest: TestBase
    {
         JT808SessionPublishingRedisImpl jT808SessionPublishingRedisImpl;

        public JT808SessionPublishingRedisImplTest()
        {
            jT808SessionPublishingRedisImpl = new JT808SessionPublishingRedisImpl(
                ServiceProvider.GetRequiredService<IOptionsMonitor<JT808Configuration>>());
        }

        [Fact]
        public void Test1()
        {  
            int i = 10000;
            Task.Run(() => {
                while (i > 0)
                {
                    jT808SessionPublishingRedisImpl.PublishAsync(JT808Constants.SessionOnline, null, Guid.NewGuid().ToString("N"));
                    jT808SessionPublishingRedisImpl.PublishAsync(JT808Constants.SessionOffline, null, Guid.NewGuid().ToString("N"));
                    i--;
                    Thread.Sleep(1000);
                }
            });
            Thread.Sleep(1000);
            List<string> SessionOnlines = new List<string>();
            ChannelMessageQueue channelMessageQueue= jT808SessionPublishingRedisImpl.Subscriber.Subscribe(JT808Constants.SessionOnline);
            channelMessageQueue.OnMessage((msg) => {
                SessionOnlines.Add(msg.Message);
            });
            List<string> SessionOfflines = new List<string>();
            ChannelMessageQueue channelMessageQueue1 = jT808SessionPublishingRedisImpl.Subscriber.Subscribe(JT808Constants.SessionOffline);
            channelMessageQueue1.OnMessage((msg) => {
                SessionOfflines.Add(msg.Message);
            });
            Thread.Sleep(3000);
        }

        [Fact]
        public void Test2()
        {
            int i = 100000;
            Task.Run(() => {
                while (i > 0)
                {
                    jT808SessionPublishingRedisImpl.PublishAsync(JT808Constants.SessionOnline, null, Guid.NewGuid().ToString("N"));
                    jT808SessionPublishingRedisImpl.PublishAsync(JT808Constants.SessionOffline, null, Guid.NewGuid().ToString("N"));
                    i--;
                    Thread.Sleep(1000);
                }
            });
            Thread.Sleep(1000);
            List<string> SessionOnlines = new List<string>();
            List<string> SessionOfflines = new List<string>();
            ChannelMessageQueue channelMessageQueue = jT808SessionPublishingRedisImpl.Subscriber.Subscribe(JT808Constants.SessionOnline);
            channelMessageQueue.OnMessage((msg) => {
                SessionOnlines.Add(msg.Message);
            });
            ChannelMessageQueue channelMessageQueue1 = jT808SessionPublishingRedisImpl.Subscriber.Subscribe(JT808Constants.SessionOffline);
            channelMessageQueue1.OnMessage((msg) => {
                SessionOfflines.Add(msg.Message);
            });    
        }
    }
}
