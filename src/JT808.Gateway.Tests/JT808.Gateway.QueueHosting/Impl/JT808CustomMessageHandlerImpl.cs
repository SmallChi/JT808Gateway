using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Transmit;
using JT808.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.QueueHosting.Impl
{
    public class JT808CustomMessageHandlerImpl : JT808MessageHandler
    {
        private readonly ILogger logger;
        public JT808CustomMessageHandlerImpl(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor,
            IJT808MsgProducer msgProducer,
            IJT808MsgReplyLoggingProducer msgReplyLoggingProducer,
            IJT808Config jT808Config) : base(jT808ConfigurationOptionsMonitor,
                msgProducer, 
                msgReplyLoggingProducer, 
                jT808Config)
        {
            logger = loggerFactory.CreateLogger<JT808CustomMessageHandlerImpl>();
            //过滤掉0x0200消息，通过服务服务进行下发应答，可以通过配置文件的方式进行增加修改(支持热更新)
            //jT808ConfigurationOptionsMonitor.CurrentValue.IgnoreMsgIdReply.Add(0x0200);
            //添加自定义消息
            HandlerDict.Add(0x9999, Msg0x9999);
        }


        /// <summary>
        /// 重写消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public override byte[] Processor(JT808HeaderPackage request)
        {
            try
            {
                var down = base.Processor(request);
                return down;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 重写自带的消息
        /// </summary>
        /// <param name="request"></param>
        public override byte[] Msg0x0200(JT808HeaderPackage request)
        {
            //logger.LogDebug("由于过滤了0x0200，网关是不会处理0x0200消息的应答");
            var data = base.Msg0x0200(request);
            return data;
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x9999(JT808HeaderPackage request)
        {
            logger.LogDebug("自定义消息");
            return default;
        }
    }
}
