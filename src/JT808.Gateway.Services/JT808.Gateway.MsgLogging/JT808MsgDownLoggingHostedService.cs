using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.MsgLogging
{
    public class JT808MsgDownLoggingHostedService : IHostedService
    {
        private readonly IJT808MsgReplyConsumer jT808MsgReplyConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808MsgDownLoggingHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgReplyConsumer jT808MsgReplyConsumer)
        {
            this.jT808MsgReplyConsumer = jT808MsgReplyConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Subscribe();
            jT808MsgReplyConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, JT808MsgLoggingType.down);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
