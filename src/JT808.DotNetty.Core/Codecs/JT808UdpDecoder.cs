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
            if (!message.Content.IsReadable()) return;
            IByteBuffer byteBuffer = message.Content;
            //过滤掉非808标准包
            //不包括头尾标识
            //（消息 ID ）2+（消息体属性）2+（终端手机号）6+（消息流水号）2+（检验码 ）1
            if (byteBuffer.ReadableBytes < 12)
            {
                byte[] buffer = new byte[byteBuffer.ReadableBytes];
                byteBuffer.ReadBytes(buffer, 0, byteBuffer.ReadableBytes);
                return;
            }
            else
            {
                byte[] buffer = new byte[byteBuffer.ReadableBytes];
                byteBuffer.ReadBytes(buffer);
                output.Add(new JT808UdpPackage(buffer, message.Sender));
            }
        }
    }
}
