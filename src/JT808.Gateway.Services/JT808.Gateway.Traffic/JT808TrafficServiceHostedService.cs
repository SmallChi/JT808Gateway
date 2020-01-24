using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Protocol.Extensions;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.Traffic
{
    public class JT808TrafficServiceHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808TrafficService jT808TrafficService;

        public JT808TrafficServiceHostedService(
            JT808TrafficService jT808TrafficService,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808TrafficService = jT808TrafficService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((item)=> {
                //string str = item.Data.ToHexString();
                jT808TrafficService.Processor(item.TerminalNo, item.Data.Length);
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
