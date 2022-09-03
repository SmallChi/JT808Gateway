using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway.ReplyMessage
{
    /// <summary>
    /// 
    /// </summary>
    public class JT808ReplyMessageHostedService : IHostedService
    {
        private IJT808MsgConsumer jT808MsgConsumer;
        private IJT808DownMessageHandler jT808ReplyMessageHandler;
        private IJT808MsgReplyProducer jT808MsgReplyProducer;
        private ILogger logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="jT808ReplyMessageHandler"></param>
        /// <param name="jT808MsgReplyProducer"></param>
        /// <param name="jT808MsgConsumer"></param>
        public JT808ReplyMessageHostedService(
            ILoggerFactory loggerFactory,
            IJT808DownMessageHandler jT808ReplyMessageHandler,                                                                                                                                                          
            IJT808MsgReplyProducer jT808MsgReplyProducer,
            IJT808MsgConsumer jT808MsgConsumer)
        {
            this.jT808MsgConsumer = jT808MsgConsumer;
            this.jT808MsgReplyProducer = jT808MsgReplyProducer;
            this.jT808ReplyMessageHandler = jT808ReplyMessageHandler;
            this.logger = loggerFactory.CreateLogger<JT808ReplyMessageHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(async (Message) =>
            {
                try
                {
                    var data = jT808ReplyMessageHandler.Processor(Message.TerminalNo, Message.Data);
                    if (data != null)
                    {
                        await jT808MsgReplyProducer.ProduceAsync(Message.TerminalNo, data);
                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "");
                }
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
