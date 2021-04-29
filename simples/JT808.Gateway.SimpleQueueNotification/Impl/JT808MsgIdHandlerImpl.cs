using JT808.Gateway.Abstractions;
using JT808.Gateway.MsgIdHandler;
using JT808.Gateway.SimpleQueueNotification.Hubs;
using JT808.Protocol.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.Gateway.SimpleQueueNotification.Impl
{
    public class JT808MsgIdHandlerImpl: IJT808UpMessageHandler
    {
        private readonly ILogger<JT808MsgIdHandlerImpl> logger;

        private readonly IHubContext<JT808MsgHub> _hubContext;

        public JT808MsgIdHandlerImpl(
            ILoggerFactory loggerFactory,
            IHubContext<JT808MsgHub> hubContext
            )
        {
            this._hubContext = hubContext;
            logger = loggerFactory.CreateLogger<JT808MsgIdHandlerImpl>();
        }

        public void Processor(string TerminalNo, byte[] Data)
        {
            try
            {
                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace($"{TerminalNo}-{Data.ToHexString()}");
                }
                _hubContext.Clients.All.SendAsync("ReceiveMessage", TerminalNo, Data.ToHexString());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
            }
        }
    }
}
