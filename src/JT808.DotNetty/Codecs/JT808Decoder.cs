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

namespace JT808.DotNetty.Codecs
{
    /// <summary>
    /// JT808解码
    /// </summary>
    internal class JT808Decoder : ByteToMessageDecoder
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
