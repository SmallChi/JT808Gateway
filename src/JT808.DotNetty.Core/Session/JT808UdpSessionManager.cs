using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Metadata;
using DotNetty.Transport.Channels;

namespace JT808.DotNetty.Core
{
    /// <summary>
    /// JT808 udp会话管理
    /// 估计要轮询下
    /// </summary>
    public class JT808UdpSessionManager
    {
        private readonly ILogger<JT808UdpSessionManager> logger;

        private readonly IJT808SessionPublishing jT808SessionPublishing;

        public JT808UdpSessionManager(
            IJT808SessionPublishing jT808SessionPublishing,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionPublishing = jT808SessionPublishing;
            logger = loggerFactory.CreateLogger<JT808UdpSessionManager>();
        }

        private ConcurrentDictionary<string, JT808UdpSession> SessionIdDict = new ConcurrentDictionary<string, JT808UdpSession>(StringComparer.OrdinalIgnoreCase);

        public int SessionCount
        {
            get
            {
                return SessionIdDict.Count;
            }
        }

        public JT808UdpSession GetSession(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo))
                return default;
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808UdpSession targetSession))
            {
                return targetSession;
            }
            else
            {
                return default;
            }
        }

        public void TryAdd(JT808UdpSession appSession)
        {  
            //1.先判断是否在缓存里面
            if(SessionIdDict.TryGetValue(appSession.TerminalPhoneNo,out JT808UdpSession jT808UdpSession))
            {
                //处理缓存
                //判断设备的终结点是否相同
                if (jT808UdpSession.Sender.Equals(appSession.Sender))
                {
                    //相同 更新最后上线时间
                    //每次使用最新的通道
                    //将设备第一次上线时间赋值给当前上线的时间
                    appSession.StartTime = jT808UdpSession.StartTime;
                    SessionIdDict.TryUpdate(appSession.TerminalPhoneNo, appSession, appSession);
                }
                else
                {
                    //不同 算成新设备上来并且推送通知
                    SessionIdDict.TryUpdate(appSession.TerminalPhoneNo, appSession, appSession);
                    jT808SessionPublishing.PublishAsync(JT808Constants.SessionOnline, appSession.TerminalPhoneNo);
                }
            }
            else
            {
                //添加缓存
                if (SessionIdDict.TryAdd(appSession.TerminalPhoneNo, appSession))
                {
                    //使用场景：
                    //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                    //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                    //有设备关联上来可以进行通知 例如：使用Redis发布订阅
                    jT808SessionPublishing.PublishAsync(JT808Constants.SessionOnline, appSession.TerminalPhoneNo);
                }
            }
        }

        public void Heartbeat(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo)) return;
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808UdpSession oldjT808Session))
            {
                oldjT808Session.LastActiveTime = DateTime.Now;
                SessionIdDict.TryUpdate(terminalPhoneNo, oldjT808Session, oldjT808Session);
            }
        }

        public JT808UdpSession RemoveSession(string terminalPhoneNo)
        {
            //设备离线可以进行通知
            //使用Redis 发布订阅
            if (string.IsNullOrEmpty(terminalPhoneNo)) return default;
            if (!SessionIdDict.TryGetValue(terminalPhoneNo, out JT808UdpSession jT808Session))
            {
                return default;
            }
            if (SessionIdDict.TryRemove(terminalPhoneNo, out JT808UdpSession jT808SessionRemove))
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

        public void RemoveSessionByChannel(IChannel channel)
        {
            //设备离线可以进行通知
            //使用Redis 发布订阅
            var terminalPhoneNos = SessionIdDict.Where(w => w.Value.Channel.Id == channel.Id).Select(s => s.Key).ToList();
            if (terminalPhoneNos.Count > 0)
            {
                foreach (var key in terminalPhoneNos)
                {
                    SessionIdDict.TryRemove(key, out JT808UdpSession jT808SessionRemove);
                }
                string nos = string.Join(",", terminalPhoneNos);
                logger.LogInformation($">>>{nos} Channel Remove.");
                jT808SessionPublishing.PublishAsync(JT808Constants.SessionOffline, nos);
            }        
        }

        public IEnumerable<JT808UdpSession> GetAll()
        {
            return SessionIdDict.Select(s => s.Value).ToList();
        }
    }
}

