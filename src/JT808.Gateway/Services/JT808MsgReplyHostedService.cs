using JT808.Gateway.Abstractions;
using JT808.Gateway.Configurations;
using JT808.Gateway.Enums;
using JT808.Gateway.Session;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Services
{
    internal class JT808MsgReplyHostedService : IHostedService
    {
        private readonly JT808SessionManager JT808SessionManager;

        private readonly IJT808MsgReplyConsumer JT808MsgReplyConsumer;
        private readonly JT808Configuration Configuration;
        public JT808MsgReplyHostedService(
            JT808Configuration  jT808Configuration,
            IJT808MsgReplyConsumer jT808MsgReplyConsumer,
            JT808SessionManager jT808SessionManager)
        {
            JT808MsgReplyConsumer = jT808MsgReplyConsumer;
            JT808SessionManager = jT808SessionManager;
            Configuration = jT808Configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(Configuration.MessageQueueType== JT808MessageQueueType.InMemory)
            {
                JT808MsgReplyConsumer.OnMessage(item =>
                {
                    JT808SessionManager.TrySendBySessionId(item.TerminalNo, item.Data);
                });
                JT808MsgReplyConsumer.Subscribe();
            }
            return Task.CompletedTask;    
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (Configuration.MessageQueueType == JT808MessageQueueType.InMemory)
            {
                JT808MsgReplyConsumer.Unsubscribe();
            }
            return Task.CompletedTask;
        }
    }
}
