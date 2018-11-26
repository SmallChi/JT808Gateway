using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JT808.DotNetty.Metadata;

namespace JT808.DotNetty
{
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
        /// 实际连接数
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
            SessionIdDict.AddOrUpdate(appSession.SessionID, appSession, (x, y) => appSession);
            TerminalPhoneNo_SessionId_Dict.AddOrUpdate(appSession.TerminalPhoneNo, appSession.SessionID, (x, y) => appSession.SessionID);
        }

        public JT808Session RemoveSessionByID(string sessionID)
        {
            if (sessionID == null) return null;
            try
            {
                if (SessionIdDict.TryRemove(sessionID, out JT808Session session))
                {
                    if (session.TerminalPhoneNo != null)
                    {
                        if (TerminalPhoneNo_SessionId_Dict.TryRemove(session.TerminalPhoneNo, out string sessionid))
                        {
                            logger.LogInformation($">>>{sessionID}-{session.TerminalPhoneNo} Session Remove.");
                        }
                    }
                    else
                    {
                        logger.LogInformation($">>>{sessionID} Session Remove.");
                    }
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
                    if (SessionIdDict.TryRemove(sessionid, out JT808Session session))
                    {
                        logger.LogInformation($">>>{sessionid}-{session.TerminalPhoneNo} Session Remove.");
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

        public IEnumerable<JT808Session> GetRealAll()
        {
            return SessionIdDict.Select(s=>s.Value);
        }

        public IEnumerable<JT808Session> GetRelevanceAll()
        {
            return SessionIdDict.Join(TerminalPhoneNo_SessionId_Dict, m => m.Key, s => s.Value, (m, s) => m.Value);
        }
      
    }
}

