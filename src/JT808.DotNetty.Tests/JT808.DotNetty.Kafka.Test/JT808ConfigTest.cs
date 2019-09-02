using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.IO;

namespace JT808.DotNetty.Kafka.Test
{
    public class JT808ConfigTest
    {
        [Fact]
        public void Test1()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
            configurationBuilder.AddJsonFile("JT808Config.json");
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.Configure<JT808MsgProducerConfig>(configurationRoot.GetSection("JT808MsgProducerConfig"));
            serviceDescriptors.Configure<JT808MsgConsumerConfig>(configurationRoot.GetSection("JT808MsgConsumerConfig"));
            serviceDescriptors.Configure<JT808MsgReplyProducerConfig>(configurationRoot.GetSection("JT808MsgReplyProducerConfig"));
            serviceDescriptors.Configure<JT808MsgReplyConsumerConfig>(configurationRoot.GetSection("JT808MsgReplyConsumerConfig"));
            serviceDescriptors.Configure<JT808SessionProducerConfig>(configurationRoot.GetSection("JT808SessionProducerConfig"));
            serviceDescriptors.Configure<JT808SessionConsumerConfig>(configurationRoot.GetSection("JT808SessionConsumerConfig"));
            var serviceProvider = serviceDescriptors.BuildServiceProvider();
            var jT808MsgProducerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808MsgProducerConfig>>();
            Assert.Equal("JT808Msg", jT808MsgProducerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9092", jT808MsgProducerConfigAccessor.Value.BootstrapServers);
            var jT808MsgConsumerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808MsgConsumerConfig>>();
            Assert.Equal("JT808Msg", jT808MsgConsumerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9092", jT808MsgConsumerConfigAccessor.Value.BootstrapServers);
            Assert.Equal("msg-group", jT808MsgConsumerConfigAccessor.Value.GroupId);
            var jT808MsgReplyProducerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808MsgReplyProducerConfig>>();
            Assert.Equal("JT808MsgReply", jT808MsgReplyProducerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9093", jT808MsgReplyProducerConfigAccessor.Value.BootstrapServers);
            var jT808MsgReplyConsumerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808MsgReplyConsumerConfig>>();
            Assert.Equal("JT808MsgReply", jT808MsgReplyConsumerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9093", jT808MsgReplyConsumerConfigAccessor.Value.BootstrapServers);
            Assert.Equal("msgreply-group", jT808MsgReplyConsumerConfigAccessor.Value.GroupId);
            var jT808SessionProducerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808SessionProducerConfig>>();
            Assert.Equal("JT808Session", jT808SessionProducerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9094", jT808SessionProducerConfigAccessor.Value.BootstrapServers);
            var jT808SessionConsumerConfigAccessor = serviceProvider.GetRequiredService<IOptions<JT808SessionConsumerConfig>>();
            Assert.Equal("JT808Session", jT808SessionConsumerConfigAccessor.Value.TopicName);
            Assert.Equal("127.0.0.1:9094", jT808SessionConsumerConfigAccessor.Value.BootstrapServers);
            Assert.Equal("session-group", jT808SessionConsumerConfigAccessor.Value.GroupId);
        }
    }
}
