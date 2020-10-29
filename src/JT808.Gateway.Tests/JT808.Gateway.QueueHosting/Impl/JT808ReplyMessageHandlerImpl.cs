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
            if (package.Header.MsgId == 0x0200)
            {
                if (package.Version == JT808Version.JTT2019)
                {
                    byte[] data = Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(package.Header.TerminalPhoneNo, new JT808_0x8001()
                    {
                        AckMsgId = package.Header.MsgId,
                        JT808PlatformResult = JT808PlatformResult.成功,
                        MsgNum = package.Header.MsgNum
                    }));
                    return data;
                }
                else
                {
                    byte[] data = Serializer.Serialize(JT808MsgId.平台通用应答.Create(package.Header.TerminalPhoneNo, new JT808_0x8001()
                    {
                        AckMsgId = package.Header.MsgId,
                        JT808PlatformResult = JT808PlatformResult.成功,
                        MsgNum = package.Header.MsgNum
                    }));
                    return data;
                }
            }
            return default;
        }
    }
}
