using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Metadata;
using DotNetty.Transport.Channels;

namespace JT808.DotNetty
{
    /// <summary>
    /// JT808会话管理
    /// </summary>
    public class JT808SessionManager
    {
        private readonly ILogger<JT808SessionManager> logger;

        public JT808SessionManager(
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808SessionManager>();
        }

        private ConcurrentDictionary<string, JT808Session> SessionIdDict = new ConcurrentDictionary<string, JT808Session>(StringComparer.OrdinalIgnoreCase);

        public int SessionCount
        {
            get
            {
                return SessionIdDict.Count;
            }
        }

        public JT808Session GetSession(string terminalPhoneNo)
        {
            if (string.IsNullOrEmpty(terminalPhoneNo))
                return default;
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808Session targetSession))
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
            if (SessionIdDict.TryGetValue(terminalPhoneNo, out JT808Session oldjT808Session))
            {
                oldjT808Session.LastActiveTime = DateTime.Now;
                SessionIdDict.TryUpdate(terminalPhoneNo, oldjT808Session, oldjT808Session);
            }
        }

        public void TryAdd(JT808Session appSession)
        {
            // 解决了设备号跟通道绑定到一起，不需要用到通道本身的SessionId
            // 不管设备下发更改了设备终端号，只要是没有在内存中就当是新的
            // todo:
            // 存在的问题：
            // 1.原先老的如何销毁
            // 2.这时候用的通道是相同的，设备终端是不同的
            if (SessionIdDict.TryAdd(appSession.TerminalPhoneNo, appSession))
            {
                //使用场景：
                //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                //todo: 有设备关联上来可以进行通知
                //todo: 使用Redis发布订阅
                //todo: 平台下发更改设备号的时候,这时候通道和设备号是绑定在一起的，那么要是同样的通道上来，是关联不到新的设备，需要考虑
            }
        }

        public JT808Session RemoveSession(string terminalPhoneNo)
        {
            //todo: 设备离线可以进行通知
            //todo: 使用Redis 发布订阅
            if (string.IsNullOrEmpty(terminalPhoneNo)) return default;
            if (!SessionIdDict.TryGetValue(terminalPhoneNo, out JT808Session jT808Session))
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
                    SessionIdDict.TryRemove(key, out JT808Session jT808SessionRemove);
                }
                logger.LogInformation($">>>{terminalPhoneNo}-{string.Join(",", terminalPhoneNos)} 1-n Session Remove.");
                return jT808Session;
            }
            else
            {
                if (SessionIdDict.TryRemove(terminalPhoneNo, out JT808Session jT808SessionRemove))
                {
                    logger.LogInformation($">>>{terminalPhoneNo} Session Remove.");
                    return jT808SessionRemove;
                }
                else
                {
                    return default;
                }
            }
        }

        internal void RemoveSessionByChannel(IChannel channel)
        {
            //todo: 设备离线可以进行通知
            //todo: 使用Redis 发布订阅
            var terminalPhoneNos = SessionIdDict.Where(w => w.Value.Channel.Id == channel.Id).Select(s => s.Key).ToList();
            foreach (var key in terminalPhoneNos)
            {
                SessionIdDict.TryRemove(key, out JT808Session jT808SessionRemove);
            }
            logger.LogInformation($">>>{string.Join(",", terminalPhoneNos)} Channel Remove.");
        }

        public IEnumerable<JT808Session> GetAll()
        {
            return SessionIdDict.Select(s => new JT808Session
            {
                Channel= s.Value.Channel,
                LastActiveTime= s.Value.LastActiveTime,
                StartTime= s.Value.StartTime,
                TerminalPhoneNo= s.Key
            }).ToList();
        }
    }
}

