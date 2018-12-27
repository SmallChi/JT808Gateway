using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core
{
    /// <summary>
    /// JT808 udp会话管理
    /// todo:估计要轮询下
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
            // 解决了设备号跟通道绑定到一起，不需要用到通道本身的SessionId
            // 不管设备下发更改了设备终端号，只要是没有在内存中就当是新的
            // 存在的问题：
            // 1.原先老的如何销毁
            // 2.这时候用的通道是相同的，设备终端是不同的
            // 当设备主动或者服务器断开以后，可以释放，这点内存忽略不计，况且更改设备号不是很频繁。
            if (SessionIdDict.TryAdd(appSession.TerminalPhoneNo, appSession))
            {
                //使用场景：
                //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                //todo: 有设备关联上来可以进行通知 例如：使用Redis发布订阅
                jT808SessionPublishing.PublishAsync(JT808Constants.SessionOnline, appSession.TerminalPhoneNo);
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
            //todo: 设备离线可以进行通知
            //todo: 使用Redis 发布订阅
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

        public IEnumerable<JT808UdpSession> GetAll()
        {
            return SessionIdDict.Select(s => s.Value).ToList();
        }
    }
}

