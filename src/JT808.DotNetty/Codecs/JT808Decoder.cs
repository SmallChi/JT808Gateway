using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.DotNetty.Internal;

namespace JT808.DotNetty.Codecs
{
    /// <summary>
    /// JT808解码
    /// </summary>
    internal class JT808Decoder : ByteToMessageDecoder
    {
        private readonly ILogger<JT808Decoder> logger;

        public JT808Decoder(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<JT808Decoder>();
        }

        private static readonly AtomicCounter MsgSuccessCounter = new AtomicCounter();

        private static readonly AtomicCounter MsgFailCounter = new AtomicCounter();

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] buffer = new byte[input.Capacity + 2];
            try
            {
                input.ReadBytes(buffer, 1, input.Capacity);
                buffer[0] = JT808Package.BeginFlag;
                buffer[input.Capacity + 1] = JT808Package.EndFlag;
                JT808Package jT808Package = JT808Serializer.Deserialize<JT808Package>(buffer);
                output.Add(jT808Package);
                MsgSuccessCounter.Increment();
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("accept package success count<<<" + MsgSuccessCounter.Count.ToString());
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
