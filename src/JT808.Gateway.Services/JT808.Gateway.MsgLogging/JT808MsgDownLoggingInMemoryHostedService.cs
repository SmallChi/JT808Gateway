using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;

namespace JT808.Gateway.MsgLogging
{
    public class JT808MsgDownLoggingInMemoryHostedService : IHostedService
    {
        private readonly IJT808MsgReplyConsumer jT808MsgReplyConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808MsgDownLoggingInMemoryHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgReplyConsumerFactory jT808MsgReplyConsumerFactory)
        {
            this.jT808MsgReplyConsumer = jT808MsgReplyConsumerFactory.Create(JT808ConsumerType.ReplyMessageLoggingConsumer);
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
