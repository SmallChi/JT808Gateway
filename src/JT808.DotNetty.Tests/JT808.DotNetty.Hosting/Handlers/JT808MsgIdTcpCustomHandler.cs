using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Hosting.Handlers
{
    public class JT808MsgIdTcpCustomHandler : JT808MsgIdTcpHandlerBase
    {
        public JT808MsgIdTcpCustomHandler(
            ILoggerFactory loggerFactory,
            JT808TcpSessionManager sessionManager) : base(sessionManager)
        {
            logger = loggerFactory.CreateLogger<JT808MsgIdTcpCustomHandler>();
        }

        private readonly ILogger<JT808MsgIdTcpCustomHandler> logger;

        public override IJT808Reply Msg0x0200(JT808Request request)
        {
            logger.LogDebug("Tcp_Msg0x0200");
            return base.Msg0x0200(request);
        }
    }
}
