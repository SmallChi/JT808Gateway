using DotNetty.Transport.Channels.Embedded;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class SeedSession
    {
        public  List<EmbeddedChannel> embeddedChannels = new List<EmbeddedChannel>();

        [Fact]
        public void Init()
        {
            for(var i = 0; i < 10; i++)
            {
                embeddedChannels.Add(new EmbeddedChannel(new JT808DefaultChannelId()));
            }
        }
    }
}
