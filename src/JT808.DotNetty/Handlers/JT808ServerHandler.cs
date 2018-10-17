using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.DotNetty.Metadata;

namespace JT808.DotNetty.Handlers
{
    internal class JT808ServerHandler : SimpleChannelInboundHandler<JT808.Protocol.JT808Package>
    {
        private readonly JT808MsgIdHandlerBase handler;
        
        private readonly JT808SessionManager jT808SessionManager;

        public JT808ServerHandler(JT808MsgIdHandlerBase handler, JT808SessionManager jT808SessionManager)
        {
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
                        ctx.WriteAndFlushAsync(Unpooled.WrappedBuffer(JT808Serializer.Serialize(jT808Package.Package)));
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
