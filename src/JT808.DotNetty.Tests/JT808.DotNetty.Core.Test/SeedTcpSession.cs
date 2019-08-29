using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Test
{
    [TestClass]
    public class SeedTcpSession
    {
        public JT808SessionManager jT80TcpSessionManager = new JT808SessionManager(
            new JT808SessionPublishingEmptyImpl(),
            new LoggerFactory());

        public SeedTcpSession()
        {
            for (var i = 0; i < 10; i++)
            {
                var channel = new EmbeddedChannel(new JT808DefaultChannelId());
                jT80TcpSessionManager.TryAdd(i.ToString(),channel);
            }
        }

        [TestMethod]
        public void Init()
        {
            for (var i = 0; i < 10; i++)
            {
                var channel = new EmbeddedChannel(new JT808DefaultChannelId());
                jT80TcpSessionManager.TryAdd(i.ToString(), channel);
            }
        }
    }
}
