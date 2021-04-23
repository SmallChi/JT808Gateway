using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Kafka;
using JT808.DotNetty.ReplyMessage;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;

namespace JT808.DotNetty.SimpleQueueService.Impl
{
    public class JT808DotNettyReplyMessageServiceInherited : JT808DotNettyReplyMessageService
    {
        public readonly ILogger<JT808DotNettyReplyMessageServiceInherited> logger;

        public JT808DotNettyReplyMessageServiceInherited(IJT808Config jT808Config,
            IJT808MsgReplyProducer jT808MsgReplyProducer,
            ILoggerFactory loggerFactory) 
            : base(jT808Config, jT808MsgReplyProducer)
        {
            logger = loggerFactory.CreateLogger<JT808DotNettyReplyMessageServiceInherited>();
        }

        public override void Processor((string TerminalNo, byte[] Data) parameter)
        {
            logger.LogDebug($"{parameter.TerminalNo}:{parameter.Data.ToHexString()}");
            base.Processor(parameter);
        }

        public override byte[] Msg0x0200(JT808HeaderPackage request)
        {
            logger.LogWarning("===========================================");
            logger.LogWarning($"{request.Header.TerminalPhoneNo}---{request.OriginalData.ToHexString()}");
            logger.LogWarning("===========================================");
            return base.Msg0x0200(request);
        }
    }
}
