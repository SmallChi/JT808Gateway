using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;

namespace JT808.DotNetty
{
    /// <summary>
    /// 抽象消息处理业务
    /// </summary>
    public abstract class JT808MsgIdHandlerBase
    {
        protected JT808SessionManager sessionManager { get; }
        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        protected JT808MsgIdHandlerBase(JT808SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
            HandlerDict = new Dictionary<JT808MsgId, Func<JT808Package, IChannelHandlerContext, JT808Package>>
            {
                {JT808MsgId.终端通用应答, Msg0x0001},
                {JT808MsgId.终端鉴权, Msg0x0102},
                {JT808MsgId.终端心跳, Msg0x0002},
                {JT808MsgId.终端注销, Msg0x0003},
                {JT808MsgId.终端注册, Msg0x0100},
                {JT808MsgId.位置信息汇报,Msg0x0200 },
                {JT808MsgId.定位数据批量上传,Msg0x0704 },
                {JT808MsgId.数据上行透传,Msg0x0900 }
            };
        }

        public Dictionary<JT808MsgId, Func<JT808Package, IChannelHandlerContext, JT808Package>> HandlerDict { get; }
        /// <summary>
        /// 终端通用应答
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0001(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0002(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            sessionManager.Heartbeat(reqJT808Package.Header.TerminalPhoneNo);
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 终端注销
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0003(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            sessionManager.RemoveSessionByTerminalPhoneNo(reqJT808Package.Header.TerminalPhoneNo);
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 终端注册
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0100(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            return JT808MsgId.终端注册应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8100()
            {
                Code = "J" + reqJT808Package.Header.TerminalPhoneNo,
                JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 终端鉴权
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0102(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            sessionManager.RegisterSession(new JT808Session(ctx.Channel, reqJT808Package.Header.TerminalPhoneNo));
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 位置信息汇报
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0200(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 定位数据批量上传
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0704(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
        /// <summary>
        /// 数据上行透传
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Package Msg0x0900(JT808Package reqJT808Package, IChannelHandlerContext ctx)
        {
            return JT808MsgId.平台通用应答.Create(reqJT808Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = reqJT808Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.Success,
                MsgNum = reqJT808Package.Header.MsgNum
            });
        }
    }
}
