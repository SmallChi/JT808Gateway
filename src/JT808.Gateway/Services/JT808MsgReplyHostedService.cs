using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Session;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Services
{
    internal class JT808MsgReplyHostedService : IHostedService
    {
        private readonly JT808SessionManager JT808SessionManager;

        private readonly IJT808MsgReplyConsumer JT808MsgReplyConsumer;

        private ILogger logger;

        public JT808MsgReplyHostedService(
            ILoggerFactory loggerFactory,
            IJT808MsgReplyConsumer jT808MsgReplyConsumer,
            JT808SessionManager jT808SessionManager)
        {
            JT808MsgReplyConsumer = jT808MsgReplyConsumer;
            JT808SessionManager = jT808SessionManager;
            logger = loggerFactory.CreateLogger<JT808MsgReplyHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            JT808MsgReplyConsumer.OnMessage(async(item) =>
            {
                try
                {
                    await JT808SessionManager.TrySendByTerminalPhoneNoAsync(item.TerminalNo, item.Data);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{item.TerminalNo}-{item.Data.ToHexString()}");
                }
            });
            JT808MsgReplyConsumer.Subscribe();
            return Task.CompletedTask;    
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            JT808MsgReplyConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
