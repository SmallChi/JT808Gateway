using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Collections.Generic;
using JT808.Protocol;
using DotNetty.Transport.Channels;

namespace JT808.DotNetty.Client.Codecs
{
    public class JT808ClientTcpDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] buffer = new byte[input.Capacity + 2];
            input.ReadBytes(buffer, 1, input.Capacity);
            buffer[0] = JT808Package.BeginFlag;
            buffer[input.Capacity + 1] = JT808Package.EndFlag;
            output.Add(buffer);
        }
    }
}
