using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.PubSub;

namespace JT808.Gateway.BusinessServices.MsgIdHandler
{
    public class JT808MsgIdHandlerHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;

        private readonly IJT808MsgIdHandler jT808MsgIdHandler;
        public JT808MsgIdHandlerHostedService(
            IJT808MsgIdHandler jT808DotNettyMsgIdHandler,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgIdHandler = jT808DotNettyMsgIdHandler;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(jT808MsgIdHandler.Processor);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
