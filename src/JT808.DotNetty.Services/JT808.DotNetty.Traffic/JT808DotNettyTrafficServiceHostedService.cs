using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Protocol.Extensions;

namespace JT808.DotNetty.Traffic
{
    public class JT808DotNettyTrafficServiceHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808DotNettyTrafficService jT808DotNettyTrafficService;

        public JT808DotNettyTrafficServiceHostedService(
            JT808DotNettyTrafficService jT808DotNettyTrafficService,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808DotNettyTrafficService = jT808DotNettyTrafficService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((item)=> {
                string str = item.Data.ToHexString();
                jT808DotNettyTrafficService.Processor(item.TerminalNo, item.Data.Length);
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
