using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Handlers
{
    internal class JT808ServerHandler : SimpleChannelInboundHandler<JT808.Protocol.JT808Package>
    {
        private readonly JT808MsgIdHandlerBase handler;

        public JT808ServerHandler(JT808MsgIdHandlerBase handler)
        {
            this.handler = handler;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808Package msg)
        {
            try
            {
                Func<JT808Package, IChannelHandlerContext, JT808Package> handlerFunc;
                if (handler.HandlerDict.TryGetValue(msg.Header.MsgId, out handlerFunc))
                {
                    JT808Package jT808Package = handlerFunc(msg, ctx);
                    if (jT808Package != null)
                    {
                        ctx.WriteAndFlushAsync(Unpooled.WrappedBuffer(JT808Serializer.Serialize(jT808Package)));
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
