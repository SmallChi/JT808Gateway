using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core
{
    internal class JT808SessionProducerDefaultImpl : IJT808SessionProducer
    {
        private readonly ILogger<JT808SessionProducerDefaultImpl> logger;
        public JT808SessionProducerDefaultImpl(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808SessionProducerDefaultImpl>();
        }

        public string TopicName => JT808NettyConstants.SessionTopic;

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
