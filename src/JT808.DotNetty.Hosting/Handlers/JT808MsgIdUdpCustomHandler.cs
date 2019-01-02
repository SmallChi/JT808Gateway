using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Hosting.Handlers
{
    public class JT808MsgIdUdpCustomHandler : JT808MsgIdUdpHandlerBase
    {
        public JT808MsgIdUdpCustomHandler(
            ILoggerFactory loggerFactory,
            JT808UdpSessionManager sessionManager) : base(sessionManager)
        {
            logger = loggerFactory.CreateLogger<JT808MsgIdUdpCustomHandler>();
        }
        
        private readonly ILogger<JT808MsgIdUdpCustomHandler> logger;

        public override JT808Response Msg0x0200(JT808Request request)
        {
            logger.LogDebug("Udp_Msg0x0200");
            return base.Msg0x0200(request);
        }
    }
}
