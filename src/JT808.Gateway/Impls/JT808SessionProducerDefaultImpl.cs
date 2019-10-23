using JT808.Gateway;
using JT808.Gateway.PubSub;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JT808.Gateway.Impls
{
    internal class JT808SessionProducerDefaultImpl : IJT808SessionProducer
    {
        private readonly ILogger<JT808SessionProducerDefaultImpl> logger;
        public JT808SessionProducerDefaultImpl(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808SessionProducerDefaultImpl>();
        }

        public string TopicName => JT808GatewayConstants.SessionTopic;

        public void Dispose()
        {
            
        }

        public Task ProduceAsync(string terminalNo, string notice)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"{terminalNo}-{notice}");
            }
            return Task.CompletedTask;
        }
    }
}
