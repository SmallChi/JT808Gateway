using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.SessionNotice
{
    public class JT808DotNettySessionNoticeHostedService : IHostedService
    {
        private readonly JT808DotNettySessionNoticeService jT808DotNettySessionNoticeService;
        private readonly IJT808SessionConsumer jT808SessionConsumer;
        public JT808DotNettySessionNoticeHostedService(
            IJT808SessionConsumer jT808SessionConsumer,
            JT808DotNettySessionNoticeService jT808DotNettySessionNoticeService)
        {
            this.jT808DotNettySessionNoticeService = jT808DotNettySessionNoticeService;
            this.jT808SessionConsumer = jT808SessionConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Subscribe();
            jT808SessionConsumer.OnMessage(jT808DotNettySessionNoticeService.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
