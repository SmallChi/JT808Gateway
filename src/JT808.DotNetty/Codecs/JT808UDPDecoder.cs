using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.DotNetty.Internal;
using JT808.DotNetty.Interfaces;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Metadata;

namespace JT808.DotNetty.Codecs
{
    /// <summary>
    /// JT808 UDP解码
    /// </summary>
    internal class JT808UDPDecoder : MessageToMessageDecoder<DatagramPacket>
    {


        protected override void Decode(IChannelHandlerContext context, DatagramPacket message, List<object> output)
        {
            IByteBuffer byteBuffer = message.Content;
            byte[] buffer = new byte[byteBuffer.ReadableBytes];
            byteBuffer.ReadBytes(buffer);
            output.Add(new JT808UDPPackage(buffer, message.Sender));
        }
    }
}
