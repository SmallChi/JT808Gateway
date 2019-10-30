using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Test
{
    public class SeedTcpSession
    {
        public JT808SessionManager jT80TcpSessionManager = new JT808SessionManager(
            new JT808SessionProducerDefaultImpl(new LoggerFactory()),
            new LoggerFactory());

        public SeedTcpSession()
        {
            for (var i = 0; i < 10; i++)
            {
                var channel = new EmbeddedChannel(new JT808DefaultChannelId());
                jT80TcpSessionManager.TryAdd(i.ToString(),channel);
            }
        }
    }
}
