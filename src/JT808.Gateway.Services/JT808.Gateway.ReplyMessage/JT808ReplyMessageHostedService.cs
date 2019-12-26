using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.ReplyMessage
{
    public class JT808ReplyMessageHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly JT808ReplyMessageService jT808ReplyMessageService;

        public JT808ReplyMessageHostedService(
            JT808ReplyMessageService jT808ReplyMessageService,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808ReplyMessageService = jT808ReplyMessageService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808ReplyMessageService.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
