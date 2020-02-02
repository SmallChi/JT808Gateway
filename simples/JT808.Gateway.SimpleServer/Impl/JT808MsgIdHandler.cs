using JT808.Gateway.MsgIdHandler;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleServer.Impl
{
    public class JT808MsgIdHandler : IJT808MsgIdHandler
    {
        private readonly ILogger Logger;
        public JT808MsgIdHandler(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("JT808MsgIdHandler");
        }
        public void Processor((string TerminalNo, byte[] Data) parameter)
        {
            Logger.LogDebug($"{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
        }
    }
}
