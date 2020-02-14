using JT808.Gateway.Abstractions;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Traffic;
using JT808.Gateway.Transmit;
using JT808.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808NormalReplyMessageHandlerImpl : JT808NormalReplyMessageHandler
    {
        private readonly ILogger logger;
        private readonly IJT808Traffic jT808Traffic;
        private readonly IJT808MsgLogging jT808MsgLogging;
        private readonly JT808TransmitService jT808TransmitService;
        public JT808NormalReplyMessageHandlerImpl(
            JT808TransmitService jT808TransmitService,
            IJT808MsgLogging jT808MsgLogging,
            IJT808Traffic jT808Traffic,
            ILoggerFactory  loggerFactory,
            IJT808Config jT808Config) : base(jT808Config)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808Traffic = jT808Traffic;
            this.jT808MsgLogging = jT808MsgLogging;
            logger =loggerFactory.CreateLogger("JT808NormalReplyMessageHandlerImpl");
            //添加自定义消息
            HandlerDict.Add(0x9999, Msg0x9999);
        }

        /// <summary>
        /// 重写消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public override byte[] Processor(JT808HeaderPackage request, IJT808Session session)
        {
            //处理上行消息
            var down = base.Processor(request, session);
            try
            {
                //AOP 可以自定义添加一些东西:上下行日志、
                logger.LogDebug("可以自定义添加一些东西:上下行日志、数据转发");
                //流量
                jT808Traffic.Increment(request.Header.TerminalPhoneNo, DateTime.Now.ToString("yyyyMMdd"), request.OriginalData.Length);
                var parameter = (request.Header.TerminalPhoneNo, request.OriginalData.ToArray());
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
        /// <param name="session"></param>
        public override byte[] Msg0x0200(JT808HeaderPackage request, IJT808Session session)
        {
            logger.LogDebug("重写自带Msg0x0200的消息");
            return base.Msg0x0200(request, session);
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x9999(JT808HeaderPackage request, IJT808Session session)
        {
            logger.LogDebug("自定义消息");
            return default;
        }
    }
}
