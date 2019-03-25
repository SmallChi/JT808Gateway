using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Collections.Generic;
using JT808.Protocol;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Client.Metadata;

namespace JT808.DotNetty.Client.Codecs
{
    public class JT808ClientTcpEncoder : MessageToByteEncoder<JT808ClientRequest>
    {
        private readonly ILogger<JT808ClientTcpEncoder> logger;

        public JT808ClientTcpEncoder(ILoggerFactory  loggerFactory)
        {
            logger=loggerFactory.CreateLogger<JT808ClientTcpEncoder>();
        }

        protected override void Encode(IChannelHandlerContext context, JT808ClientRequest message, IByteBuffer output)
        {
            if (message.Package != null)
            {
                try
                {
                    var sendData = JT808Serializer.Serialize(message.Package, message.MinBufferSize);
                    output.WriteBytes(sendData);
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
                output.WriteBytes(message.HexData);
            }
        }
    }
}
