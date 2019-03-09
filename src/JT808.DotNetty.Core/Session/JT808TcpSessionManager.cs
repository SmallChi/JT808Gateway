using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core
{
    /// <summary>
    /// JT808 Tcp会话管理
    /// </summary>
    public class JT808TcpSessionManager
    {
        private readonly ILogger<JT808TcpSessionManager> logger;

        private readonly IJT808SessionPublishing jT808SessionPublishing;

        public JT808TcpSessionManager(
            IJT808SessionPublishing jT808SessionPublishing,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionPublishing = jT808SessionPublishing;
            logger = loggerFactory.CreateLogger<JT808TcpSessionManager>();
        }

        private ConcurrentDictionary<string, JT808TcpSession> SessionIdDict = new ConcurrentDictionary<string, JT808TcpSession>(StringComparer.OrdinalIgnoreCase);

        public int SessionCount
        {
            get
            {
                return SessionIdDict.Count;
            }
        }

        public JT808TcpSession GetSession(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo))
                return default;
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808TcpSession targetSession))
            {
                return targetSession;
            }
            else
            {
                return default;
            }
        }

        public void Heartbeat(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo)) return;
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808TcpSession oldjT808Session))
            {
                oldjT808Session.LastActiveTime = DateTime.Now;
                SessionIdDict.TryUpdate(terminalPhoneNo, oldjT808Session, oldjT808Session);
            }
        }

        public void TryAdd(string terminalPhoneNo,IChannel channel)
        {
            // 解决了设备号跟通道绑定到一起，不需要用到通道本身的SessionId
            // 不管设备下发更改了设备终端号，只要是没有在内存中就当是新的
            // 存在的问题：
            // 1.原先老的如何销毁
            // 2.这时候用的通道是相同的，设备终端是不同的
            // 当设备主动或者服务器断开以后，可以释放，这点内存忽略不计，况且更改设备号不是很频繁。

            //修复第一次通过转发过来的数据，再次通过直连后通道没有改变导致下发不成功，所以每次进行通道的更新操作。
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808TcpSession oldJT808Session))
            {
                oldJT808Session.LastActiveTime = DateTime.Now;
                oldJT808Session.Channel = channel;
                SessionIdDict.TryUpdate(terminalPhoneNo, oldJT808Session, oldJT808Session);
            }
            else
            {
                JT808TcpSession jT808TcpSession = new JT808TcpSession(channel, terminalPhoneNo);
                if (SessionIdDict.TryAdd(terminalPhoneNo, jT808TcpSession))
                {
                    //使用场景：
                    //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                    //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                    //有设备关联上来可以进行通知 例如：使用Redis发布订阅
                    jT808SessionPublishing.PublishAsync(JT808Constants.SessionOnline, jT808TcpSession.TerminalPhoneNo);
                }
            }
        }

        public JT808TcpSession RemoveSession(string terminalPhoneNo)
        {
            //设备离线可以进行通知
            //使用Redis 发布订阅
            if (string.IsNullOrEmpty(terminalPhoneNo)) return default;
            if (!SessionIdDict.TryGetValue(terminalPhoneNo, out JT808TcpSession jT808Session))
            {
                return default;
            }
            // 处理转发过来的是数据 这时候通道对设备是1对多关系,需要清理垃圾数据
            //1.用当前会话的通道Id找出通过转发过来的其他设备的终端号
            var terminalPhoneNos = SessionIdDict.Where(w => w.Value.Channel.Id == jT808Session.Channel.Id).Select(s => s.Key).ToList();
            //2.存在则一个个移除 
            if (terminalPhoneNos.Count > 1)
            {
                //3.移除包括当前的设备号
                foreach (var key in terminalPhoneNos)
                {
                    SessionIdDict.TryRemove(key, out JT808TcpSession jT808SessionRemove);
                }
                string nos = string.Join(",", terminalPhoneNos);
                logger.LogInformation($">>>{terminalPhoneNo}-{nos} 1-n Session Remove.");
                jT808SessionPublishing.PublishAsync(JT808Constants.SessionOffline, nos);
                return jT808Session;
            }
            else
            {
                if (SessionIdDict.TryRemove(terminalPhoneNo, out JT808TcpSession jT808SessionRemove))
                {
                    logger.LogInformation($">>>{terminalPhoneNo} Session Remove.");
                    jT808SessionPublishing.PublishAsync(JT808Constants.SessionOffline,terminalPhoneNo);
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
            var terminalPhoneNos = SessionIdDict.Where(w => w.Value.Channel.Id == channel.Id).Select(s => s.Key).ToList();
            if (terminalPhoneNos.Count > 0)
            {
                foreach (var key in terminalPhoneNos)
                {
                    SessionIdDict.TryRemove(key, out JT808TcpSession jT808SessionRemove);
                }
                string nos = string.Join(",", terminalPhoneNos);
                logger.LogInformation($">>>{nos} Channel Remove.");
                jT808SessionPublishing.PublishAsync(JT808Constants.SessionOffline, nos);
            }      
        }

        public IEnumerable<JT808TcpSession> GetAll()
        {
            return SessionIdDict.Select(s => s.Value).ToList();
        }
    }
}

