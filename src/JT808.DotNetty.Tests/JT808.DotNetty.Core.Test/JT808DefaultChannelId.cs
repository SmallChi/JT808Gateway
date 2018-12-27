using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Test
{
    public class JT808DefaultChannelId : IChannelId
    {
        private string Id {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }

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
