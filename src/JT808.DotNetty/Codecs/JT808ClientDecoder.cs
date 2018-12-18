using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using JT808.Protocol;
using JT808.DotNetty.Metadata;

namespace JT808.DotNetty.Codecs
{
    /// <summary>
    /// JT808客户端解码(测试客户端)
    /// </summary>
    internal class JT808ClientDecoder : ByteToMessageDecoder
    {
        private static readonly ILogger<JT808ClientDecoder> logger=new LoggerFactory().CreateLogger<JT808ClientDecoder>();

        private static readonly JT808AtomicCounter MsgSuccessCounter = new JT808AtomicCounter();

        private static readonly JT808AtomicCounter MsgFailCounter = new JT808AtomicCounter();

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] buffer = new byte[input.Capacity + 2];
            try
            {
                input.ReadBytes(buffer, 1, input.Capacity);
                buffer[0] = JT808Package.BeginFlag;
                buffer[input.Capacity + 1] = JT808Package.EndFlag;
                JT808Package jT808Package = JT808Serializer.Deserialize<JT808Package>(buffer);
                MsgSuccessCounter.Increment();
                output.Add(jT808Package);
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + MsgSuccessCounter.Count.ToString());
                } 
            }
            catch (JT808.Protocol.Exceptions.JT808Exception ex)
            {
                MsgFailCounter.Increment();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + MsgFailCounter.Count.ToString());
                    logger.LogError(ex, "accept msg<<<" + buffer);
                }
            }
            catch (Exception ex)
            {
                MsgFailCounter.Increment();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + MsgFailCounter.Count.ToString());
                    logger.LogError(ex, "accept msg<<<" + buffer);
                }
            }
        }
    }
}
