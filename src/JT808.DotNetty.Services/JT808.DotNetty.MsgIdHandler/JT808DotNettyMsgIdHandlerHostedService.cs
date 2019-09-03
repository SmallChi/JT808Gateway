using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.MsgIdHandler
{
    public class JT808DotNettyMsgIdHandlerHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;

        private readonly IJT808DotNettyMsgIdHandler jT808DotNettyMsgIdHandler;
        public JT808DotNettyMsgIdHandlerHostedService(
            IJT808DotNettyMsgIdHandler jT808DotNettyMsgIdHandler,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808DotNettyMsgIdHandler = jT808DotNettyMsgIdHandler;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808DotNettyMsgIdHandler.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
