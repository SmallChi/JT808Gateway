using JT808.Gateway.MsgLogging;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808MsgLogging : IJT808MsgLogging
    {
        private readonly ILogger Logger;
        public JT808MsgLogging(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<JT808MsgLogging>();
        }
        public void Processor((string TerminalNo, byte[] Data) parameter, JT808MsgLoggingType jT808MsgLoggingType)
        {
            Logger.LogDebug($"{jT808MsgLoggingType.ToString()}-{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
        }
    }
}
