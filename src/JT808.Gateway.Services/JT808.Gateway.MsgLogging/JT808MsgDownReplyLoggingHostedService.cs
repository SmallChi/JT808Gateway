using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.MsgLogging
{
    public class JT808MsgDownReplyLoggingHostedService : IHostedService
    {
        private readonly IJT808MsgReplyLoggingConsumer jT808MsgReplyLoggingConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808MsgDownReplyLoggingHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgReplyLoggingConsumer jT808MsgReplyLoggingConsumer)
        {
            this.jT808MsgReplyLoggingConsumer = jT808MsgReplyLoggingConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyLoggingConsumer.Subscribe();
            jT808MsgReplyLoggingConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, JT808MsgLoggingType.down);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyLoggingConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
