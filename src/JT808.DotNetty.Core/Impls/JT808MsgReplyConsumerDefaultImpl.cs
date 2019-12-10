using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Services;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Impls
{
    internal class JT808MsgReplyConsumerDefaultImpl : IJT808MsgReplyConsumer
    {
        private readonly JT808MsgService JT808MsgService;

        private readonly JT808Serializer JT808Serializer;

        private Dictionary<ushort, MsgIdMethodDelegate> HandlerDict;

        private delegate byte[] MsgIdMethodDelegate(JT808HeaderPackage package);
        public JT808MsgReplyConsumerDefaultImpl(
                IJT808Config jT808Config,
                JT808MsgService jT808MsgService)
        {
            JT808MsgService = jT808MsgService;
            this.JT808Serializer = jT808Config.GetSerializer();
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
        public CancellationTokenSource Cts =>new CancellationTokenSource();

        public string TopicName => JT808NettyConstants.MsgReplyTopic;

        public void Dispose()
        {
            Cts.Dispose();
        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {
            Task.Run(() =>
            {
                foreach(var item in JT808MsgService.MsgQueue.GetConsumingEnumerable())
                {
                    try
                    {
                        var package = JT808Serializer.HeaderDeserialize(item.Data);
                        if (HandlerDict.TryGetValue(package.Header.MsgId, out var func))
                        {
                            var buffer = func(package);
                            if (buffer != null)
                            {
                                callback((item.TerminalNo, buffer));
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }, Cts.Token);
        }

        public void Subscribe()
        {
            
        }

        public void Unsubscribe()
        {
            Cts.Cancel();
        }

        /// <summary>
        /// 终端通用应答
        /// 平台无需回复
        /// 实现自己的业务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x0001(JT808HeaderPackage request)
        {
            return null;
        }
        /// <summary>
        /// 终端心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x0002(JT808HeaderPackage request)
        {
            if(request.Version== JT808Version.JTT2019)
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
        public byte[] Msg0x0003(JT808HeaderPackage request)
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
        public byte[] Msg0x0100(JT808HeaderPackage request)
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
        public byte[] Msg0x0102(JT808HeaderPackage request)
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
        public byte[] Msg0x0200(JT808HeaderPackage request)
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
        public byte[] Msg0x0704(JT808HeaderPackage request)
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
        public byte[] Msg0x0900(JT808HeaderPackage request)
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
