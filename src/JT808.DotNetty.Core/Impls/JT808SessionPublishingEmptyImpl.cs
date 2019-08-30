using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core
{
    internal class JT808SessionPublishingEmptyImpl : IJT808SessionPublishing
    {
        private readonly ILogger<JT808SessionPublishingEmptyImpl> logger;
        public JT808SessionPublishingEmptyImpl(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808SessionPublishingEmptyImpl>();
        }
        public Task PublishAsync(string topicName, string value)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"{topicName}-{value}");
            }
            return Task.CompletedTask;
        }
    }
}
