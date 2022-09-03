using JT808.Gateway.Abstractions;
using JT808.Gateway.NormalHosting.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808MsgReplyConsumer : IJT808MsgReplyConsumer
    {
        public CancellationTokenSource Cts { get; } = new CancellationTokenSource();

        public string TopicName { get; } = JT808GatewayConstants.MsgReplyTopic;

        private readonly JT808MsgReplyDataService MsgReplyDataService;

        private ILogger logger;

        public JT808MsgReplyConsumer(
            ILoggerFactory loggerFactory,
            JT808MsgReplyDataService msgReplyDataService)
        {
            MsgReplyDataService = msgReplyDataService;
            logger = loggerFactory.CreateLogger<JT808MsgReplyConsumer>();
        }

        public void Dispose()
        {
            
        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {
            new Thread(async () =>
            {
                while (!Cts.IsCancellationRequested)
                {
                    try
                    {
                        var item = await MsgReplyDataService.ReadAsync(Cts.Token);
                        callback(item);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "");
                    }
                }
            }).Start();
        }

        public void Subscribe()
        {
            
        }

        public void Unsubscribe()
        {
            
        }
    }
}
