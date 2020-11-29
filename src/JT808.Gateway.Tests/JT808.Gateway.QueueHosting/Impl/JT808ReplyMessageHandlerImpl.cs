using JT808.Gateway.Abstractions;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.QueueHosting.Impl
{
    public class JT808ReplyMessageHandlerImpl : IJT808ReplyMessageHandler
    {
        private ILogger logger;
        private JT808Serializer Serializer;

        public JT808ReplyMessageHandlerImpl(
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808ReplyMessageHandlerImpl>();
            Serializer = jT808Config.GetSerializer();
        }

        public byte[] Processor(string TerminalNo, byte[] Data)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"实现消息应答处理,{TerminalNo},{Data.ToHexString()}");
            }
            var package = Serializer.Deserialize(Data);
            if (package.Header.MsgId == 0x09999)
            {
                logger.LogDebug("====实现自定义或内部消息应答处理====");
            }
            return default;
        }
    }
}
