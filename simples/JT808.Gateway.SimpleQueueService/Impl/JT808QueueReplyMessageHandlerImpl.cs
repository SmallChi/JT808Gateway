using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleQueueService.Impl
{
    public class JT808QueueReplyMessageHandlerImpl: IJT808DownMessageHandler
    {
        private ILogger logger;

        public JT808QueueReplyMessageHandlerImpl(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808QueueReplyMessageHandlerImpl>();
        }

        public byte[] Processor(string TerminalNo, byte[] Data)
        {
            logger.LogDebug($"JT808QueueReplyMessageHandlerImpl=>{TerminalNo},{Data.ToHexString()}");
            return default;
        }
    }
}
