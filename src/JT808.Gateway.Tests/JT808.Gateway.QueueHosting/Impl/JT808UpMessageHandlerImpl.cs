using JT808.Gateway.Abstractions;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.QueueHosting.Impl
{
    public class JT808UpMessageHandlerImpl : IJT808UpMessageHandler
    {
        private ILogger logger;
        private JT808Serializer JT808Serializer;

        public JT808UpMessageHandlerImpl(
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808UpMessageHandlerImpl>();
            JT808Serializer = jT808Config.GetSerializer();
        }
        public void Processor(string TerminalNo, byte[] Data)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"实现上行消息处理,{TerminalNo},{Data.ToHexString()}");
            }
        }
    }
}
