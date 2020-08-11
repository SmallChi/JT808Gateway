using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Kafka;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.MsgIdHandler.Test
{
    public class JT808DotNettyMsgIdHandlerDefaultImpl : IJT808DotNettyMsgIdHandler
    {
        public readonly ILogger<JT808DotNettyMsgIdHandlerDefaultImpl> logger;
        public JT808DotNettyMsgIdHandlerDefaultImpl(ILoggerFactory loggerFactory,
                                                    IServiceProvider serviceProvider) {
            logger = loggerFactory.CreateLogger<JT808DotNettyMsgIdHandlerDefaultImpl>();
            Task.Run(()=> {
                while (true)
                {
                    Thread.Sleep(5000);
                    using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(new JT808MsgProducerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        TopicName = "JT808Msg"
                    }))
                    {
                        jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E, 0, 0x7E }).Wait();
                    }
                }
            });
        }

        public void Processor((string TerminalNo, byte[] Data) parameter)
        {
            logger.LogDebug($"{parameter.TerminalNo}:{parameter.Data.ToHexString()}");
        }
    }
}
