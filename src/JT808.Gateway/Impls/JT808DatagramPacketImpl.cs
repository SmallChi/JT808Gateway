using DotNetty.Buffers;
using DotNetty.Transport.Channels.Sockets;
using JT808.Gateway.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JT808.Gateway.Impls
{
    class JT808DatagramPacketImpl : IJT808DatagramPacket
    {
        public DatagramPacket Create(byte[] message, EndPoint recipient)
        {
            return new DatagramPacket(Unpooled.WrappedBuffer(message), recipient);
        }
    }
}
