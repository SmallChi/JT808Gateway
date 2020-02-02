using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.Internal;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.Gateway.InMemoryMQ.Test
{
    public class JT808MsgProducerTest
    {
        [Fact]
        public void Test1()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddServerInMemoryMQ(JT808ConsumerType.MsgIdHandlerConsumer | JT808ConsumerType.ReplyMessageConsumer);
            IServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
            IJT808MsgProducer producer = serviceProvider.GetRequiredService<IJT808MsgProducer>();
            producer.ProduceAsync("123", new byte[] { 1, 2, 3, 4 });
            IJT808MsgConsumer consumer = serviceProvider.GetRequiredService<IJT808MsgConsumer>();
            consumer.OnMessage((item) => {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            IJT808MsgConsumerFactory consumerFactory = serviceProvider.GetRequiredService<IJT808MsgConsumerFactory>();
            var msgIdHandlerConsumer = consumerFactory.Create(JT808ConsumerType.MsgIdHandlerConsumer);
            msgIdHandlerConsumer.OnMessage((item) => 
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var replyMessageConsumer = consumerFactory.Create(JT808ConsumerType.ReplyMessageConsumer);
            replyMessageConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
        }

        [Fact]
        public void Test2()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddServerInMemoryMQ(JT808ConsumerType.MsgIdHandlerConsumer,JT808ConsumerType.ReplyMessageConsumer);
            IServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
            IJT808MsgProducer producer = serviceProvider.GetRequiredService<IJT808MsgProducer>();
            producer.ProduceAsync("123", new byte[] { 1, 2, 3, 4 });
            IJT808MsgConsumer consumer = serviceProvider.GetRequiredService<IJT808MsgConsumer>();
            consumer.OnMessage((item) => {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            IJT808MsgConsumerFactory consumerFactory = serviceProvider.GetRequiredService<IJT808MsgConsumerFactory>();
            var msgIdHandlerConsumer = consumerFactory.Create(JT808ConsumerType.MsgIdHandlerConsumer);
            msgIdHandlerConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var replyMessageConsumer = consumerFactory.Create(JT808ConsumerType.ReplyMessageConsumer);
            replyMessageConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
        }

        [Fact]
        public void Test3()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddServerInMemoryMQ(JT808ConsumerType.All);
            IServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
            IJT808MsgProducer producer = serviceProvider.GetRequiredService<IJT808MsgProducer>();
            producer.ProduceAsync("123", new byte[] { 1, 2, 3, 4 });
            IJT808MsgConsumer consumer = serviceProvider.GetRequiredService<IJT808MsgConsumer>();
            consumer.OnMessage((item) => {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            IJT808MsgConsumerFactory consumerFactory = serviceProvider.GetRequiredService<IJT808MsgConsumerFactory>();
            var msgIdHandlerConsumer = consumerFactory.Create(JT808ConsumerType.MsgIdHandlerConsumer);
            msgIdHandlerConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var replyMessageConsumer = consumerFactory.Create(JT808ConsumerType.ReplyMessageConsumer);
            replyMessageConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var msgLoggingConsumer = consumerFactory.Create(JT808ConsumerType.MsgLoggingConsumer);
            msgLoggingConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var trafficConsumer = consumerFactory.Create(JT808ConsumerType.TrafficConsumer);
            trafficConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
            var transmitConsumer = consumerFactory.Create(JT808ConsumerType.TransmitConsumer);
            transmitConsumer.OnMessage((item) =>
            {
                Assert.Equal("123", item.TerminalNo);
                Assert.Equal(new byte[] { 1, 2, 3, 4 }, item.Data);
            });
        }
    }
}
