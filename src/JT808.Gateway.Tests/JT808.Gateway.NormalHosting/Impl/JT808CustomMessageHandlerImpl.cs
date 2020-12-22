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

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808CustomMessageHandlerImpl : JT808MessageHandler
    {
        private readonly ILogger logger;
        private readonly JT808TransmitService jT808TransmitService;
        private readonly IJT808MsgLogging jT808MsgLogging;

        public JT808CustomMessageHandlerImpl(
            ILoggerFactory loggerFactory,
            IJT808MsgLogging jT808MsgLogging,
            JT808TransmitService jT808TransmitService,
            IJT808Config jT808Config) : base(jT808Config)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808MsgLogging = jT808MsgLogging;
            logger = loggerFactory.CreateLogger<JT808CustomMessageHandlerImpl>();
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
            //处理上行消息
            var down = base.Processor(request);
            try
            {
                //AOP 可以自定义添加一些东西:上下行日志、
                logger.LogDebug("可以自定义添加一些东西:上下行日志、数据转发");
                var parameter = (request.Header.TerminalPhoneNo, request.OriginalData);
                //上行日志（可同步也可以使用队列进行异步）
                jT808MsgLogging.Processor(parameter, JT808MsgLoggingType.up);
                //下行日志（可同步也可以使用队列进行异步）
                jT808MsgLogging.Processor((request.Header.TerminalPhoneNo, down), JT808MsgLoggingType.down);
                //转发数据（可同步也可以使用队列进行异步）
                jT808TransmitService.SendAsync(parameter);
            }
            catch (Exception)
            {

            }
            return down;
        }

        /// <summary>
        /// 重写自带的消息
        /// </summary>
        /// <param name="request"></param>
        public override byte[] Msg0x0200(JT808HeaderPackage request)
        {
            logger.LogDebug("重写自带Msg0x0200的消息");
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
