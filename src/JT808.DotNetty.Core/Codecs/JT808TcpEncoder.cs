using DotNetty.Buffers;
using DotNetty.Codecs;
using JT808.Protocol;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Enums;
using JT808.Protocol.Interfaces;

namespace JT808.DotNetty.Core.Codecs
{
    /// <summary>
    /// tcp统一下发出口
    /// </summary>
    public class JT808TcpEncoder :  MessageToByteEncoder<IJT808Reply>
    {
        private readonly ILogger<JT808TcpEncoder> logger;

        private readonly JT808TrafficService jT808TrafficService;

        private readonly IJT808DownlinkPacket jT808DownlinkPacket;

        private readonly JT808Serializer JT808Serializer;

        public JT808TcpEncoder(
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory, 
            JT808TrafficServiceFactory jT808TrafficServiceFactory,
            IJT808DownlinkPacket jT808DownlinkPacket)
        {
            logger = loggerFactory.CreateLogger<JT808TcpEncoder>();
            this.jT808TrafficService = jT808TrafficServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.jT808DownlinkPacket = jT808DownlinkPacket;
            this.JT808Serializer = jT808Config.GetSerializer();
        }

        protected override void Encode(IChannelHandlerContext context, IJT808Reply message, IByteBuffer output)
        {
            if (message.Package != null)
            {
                try
                {
                    var sendData = JT808Serializer.Serialize(message.Package, message.MinBufferSize);
                    jT808TrafficService.SendSize(sendData.Length);
                    jT808DownlinkPacket.ProcessorAsync(sendData, JT808TransportProtocolType.tcp);
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
                jT808TrafficService.SendSize(message.HexData.Length);
                jT808DownlinkPacket.ProcessorAsync(message.HexData, JT808TransportProtocolType.tcp);
                output.WriteBytes(Unpooled.WrappedBuffer(message.HexData));
            }
        }
    }
}
