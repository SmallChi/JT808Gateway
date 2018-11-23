using DotNetty.Transport.Channels.Embedded;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class JT808SessionManagerTest
    {
        JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());

       [Fact]
        public void Test1()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAddOrUpdateSession(new Metadata.JT808Session(channel, "123456789123"));
        }

        [Fact]
        public void Test2()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAddSession(new Metadata.JT808Session(channel, "123456789123"));
        }
    }
}
