using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;

namespace JT808.Gateway.ReplyMessage
{
    public class JT808ReplyMessageInMemoryHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808ReplyMessageHandler jT808ReplyMessageHandler;

        public JT808ReplyMessageInMemoryHostedService(
            JT808ReplyMessageHandler jT808ReplyMessageHandler,
            IJT808MsgConsumerFactory  jT808MsgConsumerFactory)
        {
            this.jT808MsgConsumer = jT808MsgConsumerFactory.Create(JT808ConsumerType.ReplyMessageConsumer);
            this.jT808ReplyMessageHandler = jT808ReplyMessageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808ReplyMessageHandler.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
