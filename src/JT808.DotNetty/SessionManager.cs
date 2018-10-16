using JT808.DotNetty.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty
{
    public class SessionManager: IDisposable
    {
        private readonly ILogger<SessionManager> logger;
        private readonly JT808Configuration configuration;
        private readonly CancellationTokenSource cancellationTokenSource;
        public SessionManager(
            IOptions<JT808Configuration> jT808ConfigurationAccessor,
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<SessionManager>();
            configuration = jT808ConfigurationAccessor.Value;
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    logger.LogInformation($"Online Count>>>{SessionCount}");
                    if (SessionCount > 0)
                    {
                        logger.LogInformation($"SessionIds>>>{string.Join(",", SessionIdDict.Select(s => s.Key))}");
                        logger.LogInformation($"TerminalPhoneNos>>>{string.Join(",", TerminalPhoneNo_SessionId_Dict.Select(s => $"{s.Key}-{s.Value}"))}");
                    }
                    Thread.Sleep(configuration.SessionReportTime);
                }
            }, cancellationTokenSource.Token);
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

        public int SessionCount
        {
            get
            {
                return SessionIdDict.Count;
            }
        }

        public void RegisterSession(JT808Session appSession)
        {
            if (TerminalPhoneNo_SessionId_Dict.ContainsKey(appSession.TerminalPhoneNo))
            {
                return;
            }
            if (SessionIdDict.TryAdd(appSession.SessionID, appSession) &&
                TerminalPhoneNo_SessionId_Dict.TryAdd(appSession.TerminalPhoneNo, appSession.SessionID))
            {
                return;
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

        /// <summary>
        /// 通过通道Id和设备终端号进行关联
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="terminalPhoneNo"></param>
        public void UpdateSessionByID(string sessionID, string terminalPhoneNo)
        {
            try
            {
                if (SessionIdDict.TryGetValue(sessionID, out JT808Session oldjT808Session))
                {
                    oldjT808Session.TerminalPhoneNo = terminalPhoneNo;
                    if (SessionIdDict.TryUpdate(sessionID, oldjT808Session, oldjT808Session))
                    {
                        TerminalPhoneNo_SessionId_Dict.AddOrUpdate(terminalPhoneNo, sessionID, (tpn, sid) =>
                        {
                            return sessionID;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{sessionID},{terminalPhoneNo}");
            }
        }

        public void RemoveSessionByID(string sessionID)
        {
            if (sessionID == null) return;
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
                    // call GPS.JT808NettyServer.Handlers.JT808ConnectionHandler.CloseAsync
                    session.Channel.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $">>>{sessionID} Session Remove Exception");
            }
        }

        public void RemoveSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            if (terminalPhoneNo == null) return;
            try
            {
                if (TerminalPhoneNo_SessionId_Dict.TryRemove(terminalPhoneNo, out string sessionid))
                {
                    if (SessionIdDict.TryRemove(sessionid, out JT808Session session))
                    {
                        logger.LogInformation($">>>{terminalPhoneNo}-{sessionid} TerminalPhoneNo Remove.");
                    }
                    else
                    {
                        logger.LogInformation($">>>{terminalPhoneNo} TerminalPhoneNo Remove.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $">>>{terminalPhoneNo} TerminalPhoneNo Remove Exception.");
            }
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}

