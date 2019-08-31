using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Collections.Generic;
using JT808.Protocol;
using DotNetty.Transport.Channels;

namespace JT808.DotNetty.Core.Codecs
{
    public class JT808TcpDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            //过滤掉不是808标准包
            //不包括头尾标识
            //（消息 ID ）2+（消息体属性）2+（终端手机号）6+（消息流水号）2+（检验码 ）1
            if (input.Capacity < 12)
            {
                byte[] buffer = new byte[input.Capacity];
                input.ReadBytes(buffer, 0, input.Capacity);
                return;
            }
            else
            {
                byte[] buffer = new byte[input.Capacity + 2];
                input.ReadBytes(buffer, 1, input.Capacity);
                buffer[0] = JT808Package.BeginFlag;
                buffer[input.Capacity + 1] = JT808Package.EndFlag;
                output.Add(buffer);
            }
        }
    }
}
