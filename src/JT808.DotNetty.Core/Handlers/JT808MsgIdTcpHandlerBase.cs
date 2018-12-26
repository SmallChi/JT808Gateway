using System;
using System.Collections.Generic;
using JT808.DotNetty.Core.Metadata;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;

namespace JT808.DotNetty.Core.Handlers
{
    /// <summary>
    /// 基于Tcp模式抽象消息处理业务
    /// 自定义消息处理业务
    /// 注意:
    /// 1.ConfigureServices:
    /// services.Replace(new ServiceDescriptor(typeof(JT808MsgIdTcpHandlerBase),typeof(JT808MsgIdCustomTcpHandlerImpl),ServiceLifetime.Singleton));
    /// 2.解析具体的消息体，具体消息调用具体的JT808Serializer.Deserialize<T>
    /// </summary>
    public abstract class JT808MsgIdTcpHandlerBase
    {
        protected JT808TcpSessionManager sessionManager { get; }
        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        protected JT808MsgIdTcpHandlerBase(JT808TcpSessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
            HandlerDict = new Dictionary<ushort, Func<JT808Request, JT808Response>>
            {
                {JT808MsgId.终端通用应答.ToUInt16Value(), Msg0x0001},
                {JT808MsgId.终端鉴权.ToUInt16Value(), Msg0x0102},
                {JT808MsgId.终端心跳.ToUInt16Value(), Msg0x0002},
                {JT808MsgId.终端注销.ToUInt16Value(), Msg0x0003},
                {JT808MsgId.终端注册.ToUInt16Value(), Msg0x0100},
                {JT808MsgId.位置信息汇报.ToUInt16Value(),Msg0x0200 },
                {JT808MsgId.定位数据批量上传.ToUInt16Value(),Msg0x0704 },
                {JT808MsgId.数据上行透传.ToUInt16Value(),Msg0x0900 }
            };
        }

        public Dictionary<ushort, Func<JT808Request, JT808Response>> HandlerDict { get; protected set; }
        /// <summary>
        /// 终端通用应答
        /// 平台无需回复
        /// 实现自己的业务
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0001(JT808Request request)
        {
            return null;
        }
        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0002(JT808Request request)
        {
            sessionManager.Heartbeat(request.Package.Header.TerminalPhoneNo);
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 终端注销
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0003(JT808Request request)
        {
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 终端注册
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0100(JT808Request request)
        {
            return new JT808Response(JT808MsgId.终端注册应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8100()
            {
                Code = "J" + request.Package.Header.TerminalPhoneNo,
                JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 终端鉴权
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0102(JT808Request request)
        {
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 位置信息汇报
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0200(JT808Request request)
        {
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId = request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 定位数据批量上传
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0704(JT808Request request)
        {
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId =request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
        /// <summary>
        /// 数据上行透传
        /// </summary>
        /// <param name="reqJT808Package"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual JT808Response Msg0x0900(JT808Request request)
        {
            return new JT808Response(JT808MsgId.平台通用应答.Create(request.Package.Header.TerminalPhoneNo, new JT808_0x8001()
            {
                MsgId =request.Package.Header.MsgId,
                JT808PlatformResult = JT808PlatformResult.成功,
                MsgNum = request.Package.Header.MsgNum
            }));
        }
    }
}
