using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.ReplyMessage
{
    public class JT808DotNettyReplyMessageHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808DotNettyReplyMessageService jT808DotNettyReplyMessageService;

        public JT808DotNettyReplyMessageHostedService(
            JT808DotNettyReplyMessageService jT808DotNettyReplyMessageService,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808DotNettyReplyMessageService = jT808DotNettyReplyMessageService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808DotNettyReplyMessageService.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
