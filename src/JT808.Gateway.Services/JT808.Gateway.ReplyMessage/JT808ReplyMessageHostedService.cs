using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.ReplyMessage
{
    public class JT808ReplyMessageHostedService : IHostedService
    {
        private IJT808MsgConsumer jT808MsgConsumer;
        private IJT808ReplyMessageHandler jT808ReplyMessageHandler;

        public JT808ReplyMessageHostedService(
            IJT808ReplyMessageHandler jT808ReplyMessageHandler,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808ReplyMessageHandler = jT808ReplyMessageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((Message)=> {
                jT808ReplyMessageHandler.Processor(Message.TerminalNo, Message.Data);
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
