using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace JT808.DotNetty.Core.Session
{
    public class JT808SessionManager
    {
        private readonly ILogger<JT808SessionManager> logger;

        private readonly IJT808DatagramPacket jT808DatagramPacket;
        public IJT808SessionProducer JT808SessionProducer { get; }

        public ConcurrentDictionary<string, IJT808Session> Sessions { get; }

        public JT808SessionManager(
            IJT808SessionProducer jT808SessionProducer,
            ILoggerFactory loggerFactory
            )
        {
            Sessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            JT808SessionProducer = jT808SessionProducer;
            logger = loggerFactory.CreateLogger<JT808SessionManager>();
        }

        public JT808SessionManager(
            IJT808SessionProducer jT808SessionProducer,
            ILoggerFactory loggerFactory,
            IJT808DatagramPacket jT808DatagramPacket)
        {
            Sessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            JT808SessionProducer = jT808SessionProducer;
            logger = loggerFactory.CreateLogger<JT808SessionManager>();
            this.jT808DatagramPacket = jT808DatagramPacket;
        }

        public int SessionCount
        {
            get
            {
                return Sessions.Count;
            }
        }
        public IJT808Session GetSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo))
                return default;
            if (Sessions.TryGetValue(terminalPhoneNo, out IJT808Session targetSession))
            {
                return targetSession;
            }
            else
            {
                return default;
            }
        }
        public JT808TcpSession GetTcpSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            return (JT808TcpSession)GetSessionByTerminalPhoneNo(terminalPhoneNo);
        }
        public JT808UdpSession GetUdpSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            return (JT808UdpSession)GetSessionByTerminalPhoneNo(terminalPhoneNo);
        }
        public void Heartbeat(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo)) return;
            if (Sessions.TryGetValue(terminalPhoneNo, out IJT808Session oldjT808Session))
            {
                oldjT808Session.LastActiveTime = DateTime.Now;
                Sessions.TryUpdate(terminalPhoneNo, oldjT808Session, oldjT808Session);
            }
        }
        public bool TrySend(string terminalPhoneNo, byte[] data, out string message)
        {
            bool isSuccessed;
            var session = GetSessionByTerminalPhoneNo(terminalPhoneNo);
            if (session != null)
            {
                //判断转发数据是下发不了消息的
                if (Sessions.Select(s => s.Value).Count(c => c.Channel.Id == session.Channel.Id) > 1)
                {
                    isSuccessed = false;
                    message = "not support transmit data send.";
                }
                else
                {
                    if(session.TransportProtocolType== JT808TransportProtocolType.tcp)
                    {
                        session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                        isSuccessed = true;
                        message = "ok";
                    }
                    else if (session.TransportProtocolType == JT808TransportProtocolType.udp)
                    {
                        isSuccessed = true;
                        message = "ok";
                        session.Channel.WriteAndFlushAsync(jT808DatagramPacket.Create(data, ((JT808UdpSession)session).Sender));
                    }
                    else
                    {
                        isSuccessed = false;
                        message = "unknow type";
                    }
                }
            }
            else
            {
                isSuccessed = false;
                message = "offline";
            }
            return isSuccessed;
        }
        internal void Send(string terminalPhoneNo, byte[] data)
        {
            var session = GetSessionByTerminalPhoneNo(terminalPhoneNo);
            if (session != null)
            {
                if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                {
                    session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                }
                else if (session.TransportProtocolType == JT808TransportProtocolType.udp)
                {
                    session.Channel.WriteAndFlushAsync(jT808DatagramPacket.Create(data, ((JT808UdpSession)session).Sender));
                } 
            }
        }
        public bool TrySend(string terminalPhoneNo, IJT808Reply reply, out string message)
        {
            bool isSuccessed;
            var session = GetSessionByTerminalPhoneNo(terminalPhoneNo);
            if (session != null)
            {
                //判断转发数据是下发不了消息的
                if (Sessions.Select(s => s.Value).Count(c => c.Channel.Id == session.Channel.Id) > 1)
                {
                    isSuccessed = false;
                    message = "not support transmit data send.";
                }
                else
                {
                    if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                    {
                        isSuccessed = true;
                        message = "ok";
                        session.Channel.WriteAndFlushAsync(reply);
                    }
                    else if (session.TransportProtocolType == JT808TransportProtocolType.udp)
                    {
                        isSuccessed = true;
                        message = "ok";
                        session.Channel.WriteAndFlushAsync(jT808DatagramPacket.Create(reply.HexData, ((JT808UdpSession)session).Sender));
                    }
                    else
                    {
                        isSuccessed = false;
                        message = "unknow type";
                    }
                }
            }
            else
            {
                isSuccessed = false;
                message = "offline";
            }
            return isSuccessed;
        }
        public void TryAdd(string terminalPhoneNo, IChannel channel)
        {
            // 解决了设备号跟通道绑定到一起，不需要用到通道本身的SessionId
            // 不管设备下发更改了设备终端号，只要是没有在内存中就当是新的
            // 存在的问题：
            // 1.原先老的如何销毁
            // 2.这时候用的通道是相同的，设备终端是不同的
            // 当设备主动或者服务器断开以后，可以释放，这点内存忽略不计，况且更改设备号不是很频繁。

            //修复第一次通过转发过来的数据，再次通过直连后通道没有改变导致下发不成功，所以每次进行通道的更新操作。
            if (Sessions.TryGetValue(terminalPhoneNo, out IJT808Session oldJT808Session))
            {
                oldJT808Session.LastActiveTime = DateTime.Now;
                oldJT808Session.Channel = channel;
                Sessions.TryUpdate(terminalPhoneNo, oldJT808Session, oldJT808Session);
            }
            else
            {
                JT808TcpSession jT808TcpSession = new JT808TcpSession(channel, terminalPhoneNo);
                if (Sessions.TryAdd(terminalPhoneNo, jT808TcpSession))
                {
                    //使用场景：
                    //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                    //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                    //有设备关联上来可以进行通知 例如：使用Redis发布订阅
                    JT808SessionProducer.ProduceAsync(JT808NettyConstants.SessionOnline,jT808TcpSession.TerminalPhoneNo);
                }
            }
        }
        public void TryAdd(IChannel channel, EndPoint sender, string terminalPhoneNo)
        {
            //1.先判断是否在缓存里面
            if (Sessions.TryGetValue(terminalPhoneNo, out IJT808Session jT808UdpSession))
            {
                if(jT808UdpSession is JT808UdpSession convertSession)
                {
                    convertSession.LastActiveTime = DateTime.Now;
                    convertSession.Sender = sender;
                    convertSession.Channel = channel;
                    Sessions.TryUpdate(terminalPhoneNo, convertSession, convertSession);
                }
            }
            else
            {
                //添加缓存
                //使用场景：
                //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                //有设备关联上来可以进行通知 例如：使用Redis发布订阅
                Sessions.TryAdd(terminalPhoneNo, new JT808UdpSession(channel, sender, terminalPhoneNo));
            }
            //移动是个大的内网，不跟随下发，根本就发不出来
            //移动很多卡，存储的那个socket地址端口，有效期非常短
            //不速度快点下发，那个socket地址端口就可能映射到别的对应卡去了
            //所以此处采用跟随设备消息下发指令
            JT808SessionProducer.ProduceAsync(JT808NettyConstants.SessionOnline,terminalPhoneNo);
        }
        public IJT808Session RemoveSession(string terminalPhoneNo)
        {
            //设备离线可以进行通知
            //使用Redis 发布订阅
            if (string.IsNullOrEmpty(terminalPhoneNo)) return default;
            if (!Sessions.TryGetValue(terminalPhoneNo, out IJT808Session jT808Session))
            {
                return default;
            }
            // 处理转发过来的是数据 这时候通道对设备是1对多关系,需要清理垃圾数据
            //1.用当前会话的通道Id找出通过转发过来的其他设备的终端号
            var terminalPhoneNos = Sessions.Where(w => w.Value.Channel.Id == jT808Session.Channel.Id).Select(s => s.Key).ToList();
            //2.存在则一个个移除 
            if (terminalPhoneNos.Count > 1)
            {
                //3.移除包括当前的设备号
                foreach (var key in terminalPhoneNos)
                {
                    Sessions.TryRemove(key, out IJT808Session jT808SessionRemove);
                }
                string nos = string.Join(",", terminalPhoneNos);
                logger.LogInformation($">>>{terminalPhoneNo}-{nos} 1-n Session Remove.");
                JT808SessionProducer.ProduceAsync(JT808NettyConstants.SessionOffline, nos);
                return jT808Session;
            }
            else
            {
                if (Sessions.TryRemove(terminalPhoneNo, out IJT808Session jT808SessionRemove))
                {
                    logger.LogInformation($">>>{terminalPhoneNo} Session Remove.");
                    JT808SessionProducer.ProduceAsync(JT808NettyConstants.SessionOffline, terminalPhoneNo);
                    return jT808SessionRemove;
                }
                else
                {
                    return default;
                }
            }
        }
        public void RemoveSessionByChannel(IChannel channel)
        {
            //设备离线可以进行通知
            //使用Redis 发布订阅
            var terminalPhoneNos = Sessions.Where(w => w.Value.Channel.Id == channel.Id).Select(s => s.Key).ToList();
            if (terminalPhoneNos.Count > 0)
            {
                foreach (var key in terminalPhoneNos)
                {
                    Sessions.TryRemove(key, out IJT808Session jT808SessionRemove);
                }
                string nos = string.Join(",", terminalPhoneNos);
                logger.LogInformation($">>>{nos} Channel Remove.");
                JT808SessionProducer.ProduceAsync(JT808NettyConstants.SessionOffline, nos);
            }
        }
        public IEnumerable<IJT808Session> GetAll()
        {
            return Sessions.Select(s => s.Value).ToList();
        }
        public IEnumerable<JT808TcpSession> GetTcpAll()
        {
            return Sessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.tcp).Select(s => (JT808TcpSession)s.Value).ToList();
        }
        public IEnumerable<JT808UdpSession> GetUdpAll()
        {
            return Sessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.udp).Select(s => (JT808UdpSession)s.Value).ToList();
        }
    }
}
