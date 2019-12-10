using DotNetty.Buffers;
using DotNetty.Codecs;
using JT808.Protocol;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.Logging;
using JT808.Protocol.Interfaces;

namespace JT808.DotNetty.Core.Codecs
{
    /// <summary>
    /// tcp统一下发出口
    /// </summary>
    public class JT808TcpEncoder :  MessageToByteEncoder<IJT808Reply>
    {
        private readonly ILogger<JT808TcpEncoder> logger;

        private readonly JT808Serializer JT808Serializer;

        public JT808TcpEncoder(
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808TcpEncoder>();
            this.JT808Serializer = jT808Config.GetSerializer();
        }

        protected override void Encode(IChannelHandlerContext context, IJT808Reply message, IByteBuffer output)
        {
            if (message.Package != null)
            {
                try
                {
                    var sendData = JT808Serializer.Serialize(message.Package, minBufferSize: message.MinBufferSize);
                    output.WriteBytes(Unpooled.WrappedBuffer(sendData));
                }
                catch (JT808.Protocol.Exceptions.JT808Exception ex)
                {
                    logger.LogError(ex, context.Channel.Id.AsShortText());
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, context.Channel.Id.AsShortText());
                }
            }
            else if (message.HexData != null)
            {
                output.WriteBytes(Unpooled.WrappedBuffer(message.HexData));
            }
        }
    }
}
