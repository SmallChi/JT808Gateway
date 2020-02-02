using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.InMemoryMQ
{
    public class JT808MsgTransmitConsumer : IJT808MsgConsumer
    {
        private readonly JT808MsgTransmitService JT808MsgService;
        public CancellationTokenSource Cts => new CancellationTokenSource();
        private readonly ILogger logger;
        public string TopicName => JT808GatewayConstants.MsgTopic;
        public JT808MsgTransmitConsumer(
            JT808MsgTransmitService jT808MsgService,
            ILoggerFactory loggerFactory)
        {
            JT808MsgService = jT808MsgService;
            logger = loggerFactory.CreateLogger("JT808MsgTransmitConsumer");
        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {
            Task.Run(async() =>
            {
                while (!Cts.IsCancellationRequested)
                {
                    try
                    {
                        var item = await JT808MsgService.ReadAsync(Cts.Token);
                        callback(item);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "");
                    }
                }
            }, Cts.Token);
        }

        public void Subscribe()
        {

        }

        public void Unsubscribe()
        {
            Cts.Cancel();
        }

        public void Dispose()
        {
            Cts.Dispose();
        }
    }
}
