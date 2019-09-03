using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.MsgLogging
{
    public class JT808DotNettyMsgDownLoggingHostedService : IHostedService
    {
        private readonly IJT808MsgReplyConsumer jT808MsgReplyConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808DotNettyMsgDownLoggingHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgReplyConsumer jT808MsgReplyConsumer)
        {
            this.jT808MsgReplyConsumer = jT808MsgReplyConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Subscribe();
            jT808MsgReplyConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, JT808MsgLoggingType.down);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgReplyConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
