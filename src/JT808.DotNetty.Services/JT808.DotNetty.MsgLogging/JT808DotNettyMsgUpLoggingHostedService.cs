using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.MsgLogging
{
    public class JT808DotNettyMsgUpLoggingHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly IJT808MsgLogging jT808MsgLogging;
        public JT808DotNettyMsgUpLoggingHostedService(
            IJT808MsgLogging jT808MsgLogging,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808MsgLogging = jT808MsgLogging;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(item=> 
            {
                jT808MsgLogging.Processor(item, JT808MsgLoggingType.up);
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
