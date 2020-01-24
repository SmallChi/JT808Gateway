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
    public class JT808MsgReplyConsumer : IJT808MsgReplyConsumer
    {
        private readonly JT808ReplyMsgService JT808ReplyMsgService;
        public CancellationTokenSource Cts => new CancellationTokenSource();

        private readonly ILogger logger;

        public string TopicName => JT808GatewayConstants.MsgReplyTopic;

        public JT808MsgReplyConsumer(
            JT808ReplyMsgService jT808ReplyMsgService,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("JT808MsgReplyConsumer");
            JT808ReplyMsgService = jT808ReplyMsgService;
        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {
            Task.Run(() =>
            {
                while (!Cts.IsCancellationRequested)
                {
                    try
                    {
                        if (JT808ReplyMsgService.TryRead(out var item))
                        {
                            callback(item);
                        }
                    }
                    catch (Exception ex)
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
