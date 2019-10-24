using System.Threading.Tasks;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.PubSub;

namespace JT808.Gateway.BusinessServices.SessionNotice
{
    public class JT808SessionNoticeHostedService : IHostedService
    {
        private readonly JT808SessionNoticeService jT808DotNettySessionNoticeService;
        private readonly IJT808SessionConsumer jT808SessionConsumer;
        public JT808SessionNoticeHostedService(
            IJT808SessionConsumer jT808SessionConsumer,
            JT808SessionNoticeService jT808DotNettySessionNoticeService)
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
