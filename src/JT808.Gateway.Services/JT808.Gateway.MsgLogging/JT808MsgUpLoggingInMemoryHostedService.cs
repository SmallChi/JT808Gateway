using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;

namespace JT808.Gateway.MsgLogging
{
    public class JT808MsgUpLoggingInMemoryHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808MsgUpLoggingInMemoryHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgConsumerFactory  jT808MsgConsumerFactory)
        {
            this.jT808MsgConsumer = jT808MsgConsumerFactory.Create(JT808ConsumerType.MsgLoggingConsumer);
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, JT808MsgLoggingType.up);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
