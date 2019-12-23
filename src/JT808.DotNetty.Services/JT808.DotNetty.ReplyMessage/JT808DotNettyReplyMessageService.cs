using JT808.DotNetty.Abstractions;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.ReplyMessage
{
    public class JT808DotNettyReplyMessageService
    {
        protected Dictionary<ushort, MsgIdMethodDelegate> HandlerDict { get; }

        protected delegate byte[] MsgIdMethodDelegate(JT808HeaderPackage package);
        protected JT808Serializer JT808Serializer { get; }
        protected IJT808MsgReplyProducer JT808MsgReplyProducer { get; }
        public JT808DotNettyReplyMessageService(
            IJT808Config jT808Config, 
            IJT808MsgReplyProducer jT808MsgReplyProducer)
        {
            this.JT808Serializer = jT808Config.GetSerializer();
            this.JT808MsgReplyProducer = jT808MsgReplyProducer;
            HandlerDict = new Dictionary<ushort, MsgIdMethodDelegate> {
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

        public virtual void Processor((string TerminalNo, byte[] Data) parameter)
        {
            try
            {
                var request = JT808Serializer.HeaderDeserialize(parameter.Data);
                if (HandlerDict.TryGetValue(request.Header.MsgId, out var func))
                {
                    var buffer = func(request);
                    if (buffer != null)
                    {
                        JT808MsgReplyProducer.ProduceAsync(parameter.TerminalNo, buffer);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 终端通用应答
        /// 平台无需回复
        /// 实现自己的业务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0001(JT808HeaderPackage request)
        {
            return null;
        }
        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0002(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 终端注销
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0003(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 终端注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0100(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.终端注册应答.Create_终端注册应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8100()
                {
                    Code = "J" + request.Header.TerminalPhoneNo,
                    JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                    AckMsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.终端注册应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8100()
                {
                    Code = "J" + request.Header.TerminalPhoneNo,
                    JT808TerminalRegisterResult = JT808TerminalRegisterResult.成功,
                    AckMsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 终端鉴权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0102(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 位置信息汇报
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0200(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 定位数据批量上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0704(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
        /// <summary>
        /// 数据上行透传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual byte[] Msg0x0900(JT808HeaderPackage request)
        {
            if (request.Version == JT808Version.JTT2019)
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create_平台通用应答_2019(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
            else
            {
                return JT808Serializer.Serialize(JT808MsgId.平台通用应答.Create(request.Header.TerminalPhoneNo, new JT808_0x8001()
                {
                    AckMsgId = request.Header.MsgId,
                    JT808PlatformResult = JT808PlatformResult.成功,
                    MsgNum = request.Header.MsgNum
                }));
            }
        }
    }
}
