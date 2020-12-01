using JT808.Protocol.Extensions;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Protocol;

namespace JT808.Gateway.SimpleServer.Impl
{
    public class JT808MsgIdHandler : JT808MessageHandler
    {
        private readonly ILogger Logger;

        public JT808MsgIdHandler(
            ILoggerFactory loggerFactory,
            IJT808Config jT808Config) 
            : base(jT808Config)
        {
            Logger = loggerFactory.CreateLogger<JT808MsgIdHandler>();
        }

        public void Processor((string TerminalNo, byte[] Data) parameter)
        {
            Logger.LogDebug($"JT808MsgIdHandler=>{parameter.TerminalNo}-{parameter.Data.ToHexString()}");
        }
    }
}
