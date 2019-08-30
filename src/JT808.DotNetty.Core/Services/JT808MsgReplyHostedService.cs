using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Session;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Services
{
    internal class JT808MsgReplyHostedService : IHostedService
    {
        private readonly JT808SessionManager JT808SessionManager;

        private readonly IJT808MsgReplyConsumer JT808MsgReplyConsumer;

        public JT808MsgReplyHostedService(
            IJT808MsgReplyConsumer jT808MsgReplyConsumer,
            JT808SessionManager jT808SessionManager)
        {
            JT808MsgReplyConsumer = jT808MsgReplyConsumer;
            JT808SessionManager = jT808SessionManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            JT808MsgReplyConsumer.OnMessage(item => 
            {
                JT808SessionManager.Send(item.TerminalNo, item.Data);
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
