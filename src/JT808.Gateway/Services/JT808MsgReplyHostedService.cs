using JT808.Gateway.Abstractions;
using JT808.Gateway.Configurations;
using JT808.Gateway.Session;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Services
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
                JT808SessionManager.TrySendByTerminalPhoneNo(item.TerminalNo, item.Data);
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
