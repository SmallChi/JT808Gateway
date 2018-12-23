using JT808.DotNetty.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Internal
{
    internal class JT808SessionPublishingEmptyImpl : IJT808SessionPublishing
    {
        public Task PublishAsync(string topicName, string key, string value)
        {
            return Task.CompletedTask;
        }
    }
}
