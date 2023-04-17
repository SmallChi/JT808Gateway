using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleServer.Impl
{
    public class JT808MsgLogging : IJT808MsgLogging
    {
        private readonly ILogger Logger;
        public JT808MsgLogging(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("JT808MsgLogging");
        }
        public void Processor((string TerminalNo, byte[] Data) parameter, JT808MsgLoggingType jT808MsgLoggingType)
        {
            if(Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug($"{jT808MsgLoggingType}-{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
            }
        }
    }
}
