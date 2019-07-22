using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Collections.Generic;
using JT808.Protocol;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Client.Metadata;
using JT808.DotNetty.Client.Services;
using JT808.Protocol.Interfaces;

namespace JT808.DotNetty.Client.Codecs
{
    public class JT808ClientTcpEncoder : MessageToByteEncoder<JT808ClientRequest>
    {
        private readonly ILogger<JT808ClientTcpEncoder> logger;
        private readonly JT808SendAtomicCounterService  jT808SendAtomicCounterService;
        private readonly JT808Serializer JT808Serializer;

        public JT808ClientTcpEncoder(
            IJT808Config jT808Config,
            JT808SendAtomicCounterService jT808SendAtomicCounterService,ILoggerFactory  loggerFactory)
        {
            logger=loggerFactory.CreateLogger<JT808ClientTcpEncoder>();
            this.jT808SendAtomicCounterService = jT808SendAtomicCounterService;
            JT808Serializer = jT808Config.GetSerializer();
        }

        protected override void Encode(IChannelHandlerContext context, JT808ClientRequest message, IByteBuffer output)
        {
            if (message.Package != null)
            {
                try
                {
                    var sendData = JT808Serializer.Serialize(message.Package, message.MinBufferSize);
                    output.WriteBytes(sendData);
                    jT808SendAtomicCounterService.MsgSuccessIncrement();
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
                jT808SendAtomicCounterService.MsgSuccessIncrement();
            }
        }
    }
}
