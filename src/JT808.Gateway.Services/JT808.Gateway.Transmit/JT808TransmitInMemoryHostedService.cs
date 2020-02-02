using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;

namespace JT808.Gateway.Transmit
{
    public class JT808TransmitInMemoryHostedService : IHostedService
    {
        private readonly JT808TransmitService jT808TransmitService;
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        public JT808TransmitInMemoryHostedService(
            IJT808MsgConsumerFactory  jT808MsgConsumerFactory,
            JT808TransmitService jT808TransmitService)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808MsgConsumer = jT808MsgConsumerFactory.Create(JT808ConsumerType.TransmitConsumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808TransmitService.Send);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
