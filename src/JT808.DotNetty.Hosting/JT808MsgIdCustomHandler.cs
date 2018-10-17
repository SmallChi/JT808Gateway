using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Metadata;
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

        public override JT808Response Msg0x0102(JT808Request request)
        {
            logger.LogDebug("Msg0x0102");
            return base.Msg0x0102(request);
        }
    }
}
