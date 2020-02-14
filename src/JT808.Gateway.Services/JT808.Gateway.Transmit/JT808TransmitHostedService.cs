using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.Transmit
{
    public class JT808TransmitHostedService:IHostedService
    {
        private readonly JT808TransmitService jT808TransmitService;
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        public JT808TransmitHostedService(
            IJT808MsgConsumer jT808MsgConsumer,
            JT808TransmitService jT808TransmitService)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808TransmitService.SendAsync);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            jT808TransmitService.Stop();
            return Task.CompletedTask;
        }
    }
}
