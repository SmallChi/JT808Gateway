using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core.Codecs
{
    public class JT808UdpDecoder : MessageToMessageDecoder<DatagramPacket>
    {
        protected override void Decode(IChannelHandlerContext context, DatagramPacket message, List<object> output)
        {
            IByteBuffer byteBuffer = message.Content;
            byte[] buffer = new byte[byteBuffer.ReadableBytes];
            byteBuffer.ReadBytes(buffer);
            output.Add(new JT808UdpPackage(buffer, message.Sender));
        }
    }
}
