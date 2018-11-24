using DotNetty.Transport.Channels;
using JT808.DotNetty.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Test
{
    public class JT808DefaultChannelId : IChannelId
    {
        private static readonly JT808AtomicCounter jT808AtomicCounter = new JT808AtomicCounter();

        public string AsLongText()
        {
            return jT808AtomicCounter.Increment().ToString();
        }

        public string AsShortText()
        {
            return jT808AtomicCounter.Increment().ToString();
        }

        public int CompareTo(IChannelId other)
        {
            return  0;
        }
    }
}
