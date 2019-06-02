using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Impls
{
    class JT808UplinkPacketEmptyImpl : IJT808UplinkPacket
    {
        public Task ProcessorAsync(byte[] data, JT808TransportProtocolType transportProtocolType)
        {
            return Task.CompletedTask;
        }
    }
}
