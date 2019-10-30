using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Impls
{
    /// <summary>
    /// 仅测试用
    /// </summary>
    internal class JT808DefaultChannelId : IChannelId
    {
        public JT808DefaultChannelId() 
        {
            Id= Guid.NewGuid().ToString("N");
        }
        public JT808DefaultChannelId(string id) 
        {
            Id = id;
        }
        private string Id { get;}

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
