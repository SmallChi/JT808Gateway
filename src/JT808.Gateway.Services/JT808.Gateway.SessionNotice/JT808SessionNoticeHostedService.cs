using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.SessionNotice
{
    public class JT808SessionNoticeHostedService : IHostedService
    {
        private readonly JT808SessionNoticeService jT808SessionNoticeService;
        private readonly IJT808SessionConsumer jT808SessionConsumer;
        public JT808SessionNoticeHostedService(
            IJT808SessionConsumer jT808SessionConsumer,
            JT808SessionNoticeService jT808SessionNoticeService)
        {
            this.jT808SessionNoticeService = jT808SessionNoticeService;
            this.jT808SessionConsumer = jT808SessionConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Subscribe();
            jT808SessionConsumer.OnMessage(jT808SessionNoticeService.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808SessionConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
