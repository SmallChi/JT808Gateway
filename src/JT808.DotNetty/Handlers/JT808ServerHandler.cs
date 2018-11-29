using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.DotNetty.Metadata;
using JT808.DotNetty.Internal;

namespace JT808.DotNetty.Handlers
{
    internal class JT808ServerHandler : SimpleChannelInboundHandler<JT808.Protocol.JT808Package>
    {
        private readonly JT808MsgIdHandlerBase handler;
        
        private readonly JT808SessionManager jT808SessionManager;

        private readonly JT808RemoteAddressTransmitConfigurationService jT808RemoteAddressTransmitConfigurationService;

        public JT808ServerHandler(
            JT808RemoteAddressTransmitConfigurationService jT808RemoteAddressTransmitConfigurationService,
            JT808MsgIdHandlerBase handler, 
            JT808SessionManager jT808SessionManager)
        {
            this.jT808RemoteAddressTransmitConfigurationService = jT808RemoteAddressTransmitConfigurationService;
            this.handler = handler;
            this.jT808SessionManager = jT808SessionManager;
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808Package msg)
        {
            try
            {
                jT808SessionManager.TryAddOrUpdateSession(new JT808Session(ctx.Channel, msg.Header.TerminalPhoneNo));
                Func<JT808Request, JT808Response> handlerFunc;
                if (handler.HandlerDict.TryGetValue(msg.Header.MsgId, out handlerFunc))
                {
                    JT808Response jT808Package = handlerFunc(new JT808Request(msg));
                    if (jT808Package != null)
                    {
                        if (!jT808RemoteAddressTransmitConfigurationService.Contains(ctx.Channel.RemoteAddress))
                        {
                            ctx.WriteAndFlushAsync(Unpooled.WrappedBuffer(JT808Serializer.Serialize(jT808Package.Package, jT808Package.MinBufferSize)));
                        }
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
