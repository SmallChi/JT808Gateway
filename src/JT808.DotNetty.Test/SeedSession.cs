using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class SeedSession
    {
        public JT808SessionManager jT808SessionManager = new JT808SessionManager(
            new JT808SessionPublishingEmptyImpl(),
            new LoggerFactory());

        public SeedSession()
        {
            for (var i = 0; i < 10; i++)
            {
                var channel = new EmbeddedChannel(new JT808DefaultChannelId());
                jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, i.ToString()));
            }
        }

        [Fact]
        public void Init()
        {
            for (var i = 0; i < 10; i++)
            {
                var channel = new EmbeddedChannel(new JT808DefaultChannelId());
                jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, i.ToString()));
            }
        }
    }
}
