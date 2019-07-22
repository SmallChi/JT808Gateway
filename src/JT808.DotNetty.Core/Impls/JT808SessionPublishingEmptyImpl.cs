using JT808.DotNetty.Abstractions;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core
{
    internal class JT808SessionPublishingEmptyImpl : IJT808SessionPublishing
    {
        public Task PublishAsync(string topicName, string value)
        {
            return Task.CompletedTask;
        }
    }
}
