using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Transmit;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleServer.Impl
{
    public class JT808MessageHandlerImpl : JT808MessageHandler
    {
        private readonly ILogger logger;
        private readonly IJT808MsgLogging jT808MsgLogging;
        private readonly JT808TransmitService jT808TransmitService;

        public JT808MessageHandlerImpl(
            ILoggerFactory loggerFactory,
            JT808TransmitService jT808TransmitService,
            IJT808MsgLogging jT808MsgLogging,
            IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor, IJT808MsgProducer msgProducer, IJT808MsgReplyLoggingProducer msgReplyLoggingProducer, IJT808Config jT808Config) 
            : base(jT808Config)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808MsgLogging = jT808MsgLogging;
            logger = loggerFactory.CreateLogger<JT808MessageHandlerImpl>();
            //添加自定义消息
            HandlerDict.Add(0x9999, Msg0x9999);
        }

        /// <summary>
        /// 重写消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public override byte[] Processor(in JT808HeaderPackage request)
        {
            //AOP 可以自定义添加一些东西:上下行日志、数据转发
            logger.LogDebug("可以自定义添加一些东西:上下行日志、数据转发");
            var parameter = (request.Header.TerminalPhoneNo, request.OriginalData);
            //转发数据（可同步也可以使用队列进行异步）
            try
            {
                jT808TransmitService.SendAsync(parameter);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"");
            }
            //上行日志（可同步也可以使用队列进行异步）
            jT808MsgLogging.Processor(parameter, JT808MsgLoggingType.up);
            //处理上行消息
            var down= base.Processor(request);
            //下行日志（可同步也可以使用队列进行异步）
            jT808MsgLogging.Processor((request.Header.TerminalPhoneNo, down), JT808MsgLoggingType.down);
            return down;
        }

        /// <summary>
        /// 重写自带的消息
        /// </summary>
        /// <param name="request"></param>
        public override byte[] Msg0x0200(JT808HeaderPackage request)
        {
            //logger.LogDebug("重写自带Msg0x0200的消息");
            logger.LogDebug($"重写自带Msg0x0200的消息{request.Header.TerminalPhoneNo}-{request.OriginalData.ToHexString()}");   
            return base.Msg0x0200(request);
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
