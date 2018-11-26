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

        private string Id { get { return jT808AtomicCounter.Increment().ToString(); } }

        public string AsLongText()
        {
            return Id;
        }

        public string AsShortText()
        {
            return Id;
        }

        public int CompareTo(IChannelId other)
        {
            if(other.AsShortText()== Id)
            {
                return 1;
            }
            return  0;
        }
    }
}
