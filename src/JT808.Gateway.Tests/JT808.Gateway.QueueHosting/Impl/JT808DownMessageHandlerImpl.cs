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
    public class JT808DownMessageHandlerImpl : IJT808DownMessageHandler
    {
        private ILogger logger;
        private JT808Serializer JT808Serializer;

        public JT808DownMessageHandlerImpl(
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808DownMessageHandlerImpl>();
            JT808Serializer = jT808Config.GetSerializer();
        }

        public byte[] Processor(string TerminalNo, byte[] Data)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"实现消息应答处理,{TerminalNo},{Data.ToHexString()}");
            }
            var package = JT808Serializer.Deserialize(Data);
            if (package.Header.MsgId == 0x09999)
            {
                logger.LogDebug("====实现自定义或内部消息应答处理====");
            }
            return default;
            //if (package.Version == JT808Version.JTT2019)
            //{
            //    byte[] data = JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(package.Header.TerminalPhoneNo, new JT808_0x8001()
            //    {
            //        AckMsgId = package.Header.MsgId,
            //        JT808PlatformResult = JT808PlatformResult.成功,
            //        MsgNum = package.Header.MsgNum
            //    }));
            //    return data;
            //}
            //else
            //{
            //    byte[] data = JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(package.Header.TerminalPhoneNo, new JT808_0x8001()
            //    {
            //        AckMsgId = package.Header.MsgId,
            //        JT808PlatformResult = JT808PlatformResult.成功,
            //        MsgNum = package.Header.MsgNum
            //    }));
            //    return data;
            //}
        }
    }
}
