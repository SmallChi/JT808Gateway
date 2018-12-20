using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Metadata;

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

        /// <summary>
        /// Netty生成的sessionID和Session的对应关系
        /// key = seession id
        /// value = Session
        /// </summary>
        private ConcurrentDictionary<string, JT808Session> SessionIdDict = new ConcurrentDictionary<string, JT808Session>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 终端手机号和netty生成的sessionID的对应关系
        /// key = 终端手机号
        /// value = seession id
        /// </summary>
        private ConcurrentDictionary<string, string> TerminalPhoneNo_SessionId_Dict = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 获取实际连接数
        /// </summary>
        public int RealSessionCount
        {
            get
            {
                return SessionIdDict.Count;
            }
        }

        /// <summary>
        /// 获取设备相关连的连接数
        /// </summary>
        public int RelevanceSessionCount
        {
            get
            {
                return TerminalPhoneNo_SessionId_Dict.Count;
            }
        }

        public JT808Session GetSessionByID(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID))
                return default;
            JT808Session targetSession;
            SessionIdDict.TryGetValue(sessionID, out targetSession);
            return targetSession;
        }

        public JT808Session GetSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            try
            {
                if (string.IsNullOrEmpty(terminalPhoneNo))
                    return default;
                if (TerminalPhoneNo_SessionId_Dict.TryGetValue(terminalPhoneNo, out string sessionId))
                {
                    if (SessionIdDict.TryGetValue(sessionId, out JT808Session targetSession))
                    {
                        return targetSession;
                    }
                    else
                    {
                        return default;
                    }
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, terminalPhoneNo);
                return default;
            }
        }

        public void Heartbeat(string terminalPhoneNo)
        {
            try
            {
                if (TerminalPhoneNo_SessionId_Dict.TryGetValue(terminalPhoneNo, out string sessionId))
                {
                    if (SessionIdDict.TryGetValue(sessionId, out JT808Session oldjT808Session))
                    {
                        if (oldjT808Session.Channel.Active)
                        {
                            oldjT808Session.LastActiveTime = DateTime.Now;
                            if (SessionIdDict.TryUpdate(sessionId, oldjT808Session, oldjT808Session))
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, terminalPhoneNo);
            }
        }

        public void TryAddOrUpdateSession(JT808Session appSession)
        {
            SessionIdDict.TryAdd(appSession.SessionID, appSession);
            if(TerminalPhoneNo_SessionId_Dict.TryAdd(appSession.TerminalPhoneNo, appSession.SessionID))
            {
                //使用场景：
                //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
                //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
                //todo: 有设备关联上来可以进行通知
                //todo: 使用Redis发布订阅
                
            }
        }

        public JT808Session RemoveSessionByID(string sessionID)
        {
            if (sessionID == null) return null;
            try
            {
                if (SessionIdDict.TryRemove(sessionID, out JT808Session session))
                {
                    // 处理转发过来的是数据 这时候通道对设备是1对多关系
                    var removeKeys = TerminalPhoneNo_SessionId_Dict.Where(s => s.Value == sessionID).Select(s => s.Key).ToList();
                    foreach(var key in removeKeys)
                    {
                        TerminalPhoneNo_SessionId_Dict.TryRemove(key, out string sessionid);
                    }
                    //todo: 设备离线可以进行通知
                    //todo: 使用Redis 发布订阅

                    logger.LogInformation($">>>{sessionID}-{string.Join(",",removeKeys)} Session Remove.");
                    return session;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $">>>{sessionID} Session Remove Exception");
            }
            return null;
        }

        public JT808Session RemoveSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            if (terminalPhoneNo == null) return null;
            try
            {
                if (TerminalPhoneNo_SessionId_Dict.TryRemove(terminalPhoneNo, out string sessionid))
                {
                    // 处理转发过来的是数据 这时候通道对设备是1对多关系
                    var removeKeys = TerminalPhoneNo_SessionId_Dict.Where(w => w.Value == sessionid).Select(s=>s.Key).ToList();
                    if (removeKeys.Count > 0)
                    {
                        foreach (var key in removeKeys)
                        {
                            TerminalPhoneNo_SessionId_Dict.TryRemove(key, out string sessionid1);
                        }
                        logger.LogInformation($">>>{sessionid}-{string.Join(",", removeKeys)} 1-n Session Remove.");
                    }
                    if (SessionIdDict.TryRemove(sessionid, out JT808Session session))
                    {
                        logger.LogInformation($">>>{sessionid}-{session.TerminalPhoneNo} 1-1 Session Remove.");
                        return session;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $">>>{terminalPhoneNo} Session Remove Exception");
            }
            return null;
        }

        public IEnumerable<JT808Session> GetAll()
        {
            return TerminalPhoneNo_SessionId_Dict.Join(SessionIdDict, m => m.Value, s => s.Key, (m, s) => new JT808Session
            {
                Channel= s.Value.Channel,
                LastActiveTime= s.Value.LastActiveTime,
                SessionID= s.Value.SessionID,
                StartTime= s.Value.StartTime,
                TerminalPhoneNo= m.Key
            }).ToList();
        }
    }
}

