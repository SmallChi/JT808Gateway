using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using Microsoft.Extensions.Logging;

namespace JT808.DotNetty.Hosting
{
    public class JT808MsgIdCustomHandler : JT808MsgIdHandlerBase
    {
        private readonly ILogger<JT808MsgIdCustomHandler> logger;
        public JT808MsgIdCustomHandler(
            ILoggerFactory loggerFactory,
            JT808SessionManager sessionManager) : base(sessionManager)
        {
            logger = loggerFactory.CreateLogger<JT808MsgIdCustomHandler>();
        }

        public override JT808Package Msg0x0102(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            logger.LogDebug("Msg0x0102");
            return base.Msg0x0102(reqJT808Package, ctx);
        }
    }
}
