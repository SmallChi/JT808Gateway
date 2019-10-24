using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.PubSub;

namespace JT808.Gateway.BusinessServices.ReplyMessage
{
    public class JT808ReplyMessageHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808ReplyMessageService jT808DotNettyReplyMessageService;

        public JT808ReplyMessageHostedService(
            JT808ReplyMessageService jT808DotNettyReplyMessageService,
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
