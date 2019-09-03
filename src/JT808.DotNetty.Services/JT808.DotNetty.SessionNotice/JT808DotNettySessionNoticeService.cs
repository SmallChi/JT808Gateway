using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.SessionNotice
{
    public class JT808DotNettySessionNoticeService
    {
        protected ILogger logger { get; }
        public JT808DotNettySessionNoticeService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("JT808DotNettySessionNoticeService");
        }
        public virtual void Processor((string Notice, string TerminalNo) parameter)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"{parameter.Notice}-{parameter.TerminalNo}");
            }
        }
    }
}
