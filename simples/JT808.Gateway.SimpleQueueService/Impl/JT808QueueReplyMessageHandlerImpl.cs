using JT808.Gateway.Abstractions;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleQueueService.Impl
{
    public class JT808QueueReplyMessageHandlerImpl : JT808QueueReplyMessageHandler
    {
        public JT808QueueReplyMessageHandlerImpl(IJT808Config jT808Config, IJT808MsgReplyProducer jT808MsgReplyProducer) : base(jT808Config, jT808MsgReplyProducer)
        {
            //添加自定义消息
            HandlerDict.Add(0x9999, Msg0x9999);
        }

        /// <summary>
        /// 重写消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override byte[] Msg0x0001(JT808HeaderPackage request)
        {
            return base.Msg0x0001(request);
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x9999(JT808HeaderPackage request)
        {
            return default;
        }
    }
}
