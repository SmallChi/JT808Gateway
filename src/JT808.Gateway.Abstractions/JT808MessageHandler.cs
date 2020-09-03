using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    /// <summary>
    /// 通用消息处理程序
    /// </summary>
    public class JT808MessageHandler
    {
        protected Dictionary<ushort, MsgIdMethodDelegate> HandlerDict { get; }

        protected delegate byte[] MsgIdMethodDelegate(JT808HeaderPackage package, IJT808Session session);
        protected JT808Serializer JT808Serializer { get; }
        public JT808MessageHandler(IJT808Config jT808Config)
        {
            this.JT808Serializer = jT808Config.GetSerializer();
            HandlerDict = new Dictionary<ushort, MsgIdMethodDelegate> {
                {JT808MsgId.终端通用应答.ToUInt16Value(), Msg0x0001},
                {JT808MsgId.终端鉴权.ToUInt16Value(), Msg0x0102},
                {JT808MsgId.终端心跳.ToUInt16Value(), Msg0x0002},
                {JT808MsgId.终端注销.ToUInt16Value(), Msg0x0003},
                {JT808MsgId.终端注册.ToUInt16Value(), Msg0x0100},
                {JT808MsgId.位置信息汇报.ToUInt16Value(),Msg0x0200 },
                {JT808MsgId.定位数据批量上传.ToUInt16Value(),Msg0x0704 },
                {JT808MsgId.数据上行透传.ToUInt16Value(),Msg0x0900 },
                {JT808MsgId.查询服务器时间请求.ToUInt16Value(),Msg0x0004 },
                {JT808MsgId.查询终端参数应答.ToUInt16Value(),Msg0x0104 },
                {JT808MsgId.查询终端属性应答.ToUInt16Value(),Msg0x0107 },
                {JT808MsgId.终端升级结果通知.ToUInt16Value(),Msg0x0108 },
                {JT808MsgId.位置信息查询应答.ToUInt16Value(),Msg0x0201 },
                {JT808MsgId.链路检测.ToUInt16Value(),Msg0x8204 },
                {JT808MsgId.车辆控制应答.ToUInt16Value(),Msg0x0500 },
                {JT808MsgId.摄像头立即拍摄命令.ToUInt16Value(),Msg0x8801 },
                {JT808MsgId.多媒体数据上传.ToUInt16Value(),Msg0x0801 },
                {JT808MsgId.多媒体事件信息上传.ToUInt16Value(),Msg0x0800 },
                {JT808MsgId.CAN总线数据上传.ToUInt16Value(),Msg0x0705 },           
            };
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="session">当前会话</param>
        /// <returns>应答消息数据</returns>
        public virtual byte[] Processor(JT808HeaderPackage request, IJT808Session session)
        {
            if (HandlerDict.TryGetValue(request.Header.MsgId, out var func))
            {
                return func(request, session);
            }
            return default;
            //else
            //{
            //    //处理不了的消息统一回复通用应答
            //    return CommonReply(request, session);
            //}
        }
        /// <summary>
        /// 终端通用应答
        /// 平台无需回复
        /// 实现自己的业务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0001(JT808HeaderPackage request, IJT808Session session)
        {
            return default;
        }
        /// <summary>
        /// 平台通用应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public virtual byte[] CommonReply(JT808HeaderPackage request, IJT808Session session)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                byte[] data = JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
                session.Send(data);
                return data;
            }
            else
            {
                byte[] data = JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
                session.Send(data);
                return data;
            }
        }
        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0002(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 查询服务器时间
        /// 2019版本
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0004(JT808HeaderPackage request, IJT808Session session)
        {
            byte[] data = JT808Serializer.Serialize(JT808MsgId.查询服务器时间应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8004()
            {
                 Time=DateTime.Now
            }));
            session.Send(data);
            return data;
        }
        /// <summary>
        /// 服务器补传分包请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x8003(JT808HeaderPackage request, IJT808Session session)
        {
            throw new NotImplementedException("0x8003-服务器补传分包请求");
        }
        /// <summary>
        /// 终端补传分包请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0005(JT808HeaderPackage request, IJT808Session session)
        {
            throw new NotImplementedException("0x0005-终端补传分包请求");
        }
        /// <summary>
        /// 终端注册
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0100(JT808HeaderPackage request, IJT808Session session)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                byte[] data = JT808Serializer.Serialize(JT808MsgId.终端注册应答.Create_终端注册应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8100()
                {
                    Code = "J" + request.Header.TerminalPhoneNo,
                    JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                    AckMsgNum = request.Header.MsgNum
                }));
                session.Send(data);
                return data;
            }
            else
            {
                byte[] data = JT808Serializer.Serialize(JT808MsgId.终端注册应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8100()
                {
                    Code = "J" + request.Header.TerminalPhoneNo,
                    JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                    AckMsgNum = request.Header.MsgNum
                }));
                session.Send(data);
                return data;
            }
        }
        /// <summary>
        /// 终端注销
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0003(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 终端鉴权
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0102(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 查询终端参数应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0104(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 查询终端属性应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0107(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 终端升级结果应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0108(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 位置信息汇报
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0200(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 位置信息查询应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0201(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 链路检测
        /// 2019版本
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x8204(JT808HeaderPackage request, IJT808Session session)
        {
            return default;
        }
        /// <summary>
        /// 车辆控制应答
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0500(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 定位数据批量上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0704(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// CAN总线数据上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0705(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 多媒体事件信息上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0800(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
        /// <summary>
        /// 多媒体数据上传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0801(JT808HeaderPackage request, IJT808Session session)
        {
            throw new NotImplementedException("0x8800多媒体数据上传应答");
        }
        /// <summary>
        /// 摄像头立即拍摄命令
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x8801(JT808HeaderPackage request, IJT808Session session)
        {
            throw new NotImplementedException("0x0805摄像头立即拍摄命令应答");
        }
        /// <summary>
        /// 数据上行透传
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0900(JT808HeaderPackage request, IJT808Session session)
        {
            return CommonReply(request, session);
        }
    }
}
