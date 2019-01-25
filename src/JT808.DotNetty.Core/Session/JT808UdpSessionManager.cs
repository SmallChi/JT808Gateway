using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Metadata;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Core.Configurations;
using Microsoft.Extensions.Options;

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
        private readonly IOptionsMonitor<JT808Configuration> jT808ConfigurationAccessor;
        public JT808UdpSessionManager(
            IJT808SessionPublishing jT808SessionPublishing,
            IOptionsMonitor<JT808Configuration> jT808ConfigurationAccessor,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionPublishing = jT808SessionPublishing;
            logger = loggerFactory.CreateLogger<JT808UdpSessionManager>();
           this.jT808ConfigurationAccessor = jT808ConfigurationAccessor;
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
            if (SessionIdDict.TryGetValue(appSession.TerminalPhoneNo,out JT808UdpSession jT808UdpSession))
            {
                SessionIdDict.TryUpdate(appSession.TerminalPhoneNo, appSession, appSession);
            }
            else
            {
                //添加缓存
                //使用场景：
                //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                //有设备关联上来可以进行通知 例如：使用Redis发布订阅
                SessionIdDict.TryAdd(appSession.TerminalPhoneNo, appSession);
            }
            //移动是个大的内网，不跟随下发，根本就发不出来
            //移动很多卡，存储的那个socket地址端口，有效期非常短
            //不速度快点下发，那个socket地址端口就可能映射到别的对应卡去了
            //所以此处采用跟随设备消息下发指令
            jT808SessionPublishing.PublishAsync(JT808Constants.SessionOnline, appSession.TerminalPhoneNo);
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

