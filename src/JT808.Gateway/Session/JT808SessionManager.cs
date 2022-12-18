using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway.Session
{
    /// <summary>
    /// 
    /// <remark>不支持变态类型:既发TCP和UDP</remark>
    /// </summary>
    public class JT808SessionManager
    {
        private readonly ILogger logger;
        private readonly IJT808SessionProducer SessionProducer;
        /// <summary>
        /// socket连接会话
        /// </summary>
        public ConcurrentDictionary<string, IJT808Session> Sessions { get; }
        /// <summary>
        /// socket绑定的终端SIM连接会话
        /// </summary>
        public ConcurrentDictionary<string, IJT808Session> TerminalPhoneNoSessions { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808SessionProducer"></param>
        /// <param name="loggerFactory"></param>
        public JT808SessionManager(
            IJT808SessionProducer jT808SessionProducer,
            ILoggerFactory loggerFactory
            )
        {
            SessionProducer = jT808SessionProducer;
            Sessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            TerminalPhoneNoSessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            logger = loggerFactory.CreateLogger<JT808SessionManager>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        public JT808SessionManager(ILoggerFactory loggerFactory)
        {
            Sessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            TerminalPhoneNoSessions = new ConcurrentDictionary<string, IJT808Session>(StringComparer.OrdinalIgnoreCase);
            logger = loggerFactory.CreateLogger<JT808SessionManager>();
        }
        /// <summary>
        /// 获取会话总数量
        /// </summary>
        public int TotalSessionCount
        {
            get
            {
                return Sessions.Count;
            }
        }
        /// <summary>
        /// 获取tcp会话数量
        /// </summary>
        public int TcpSessionCount
        {
            get
            {
                return Sessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.tcp).Count();
            }
        }
        /// <summary>
        /// 获取udp会话数量
        /// </summary>
        public int UdpSessionCount
        {
            get
            {
                return Sessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.udp).Count();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <param name="session"></param>
        internal void TryLink(string terminalPhoneNo, IJT808Session session)
        {
            session.TerminalPhoneNo = terminalPhoneNo;
            DateTime curretDatetime = DateTime.Now;
            if (TerminalPhoneNoSessions.TryGetValue(terminalPhoneNo, out IJT808Session cacheSession))
            {
                if (session.SessionID != cacheSession.SessionID)
                {
                    //从转发到直连的数据需要更新缓存
                    session.ActiveTime = curretDatetime;
                    TerminalPhoneNoSessions.TryUpdate(terminalPhoneNo, session, cacheSession);
                    cacheSession.Close();
                    //会话通知
                    if (SessionProducer != null)
                    {
                        SessionProducer.ProduceAsync(JT808GatewayConstants.SessionOnline, terminalPhoneNo);
                    }
                }
                else
                {
                    cacheSession.ActiveTime = curretDatetime;
                    TerminalPhoneNoSessions.TryUpdate(terminalPhoneNo, cacheSession, cacheSession);
                }
            }
            else
            {
                if (TerminalPhoneNoSessions.TryAdd(terminalPhoneNo, session))
                {
                    //会话通知
                    if (SessionProducer != null)
                    {
                        SessionProducer.ProduceAsync(JT808GatewayConstants.SessionOnline, terminalPhoneNo);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <param name="socket"></param>
        /// <param name="remoteEndPoint"></param>
        /// <returns></returns>
        public IJT808Session TryLink(string terminalPhoneNo, Socket socket, EndPoint remoteEndPoint)
        {
            if (TerminalPhoneNoSessions.TryGetValue(terminalPhoneNo, out IJT808Session currentSession))
            {
                currentSession.ActiveTime = DateTime.Now;
                currentSession.TerminalPhoneNo = terminalPhoneNo;
                currentSession.RemoteEndPoint = remoteEndPoint;
                TerminalPhoneNoSessions.TryUpdate(terminalPhoneNo, currentSession, currentSession);
            }
            else
            {
                JT808UdpSession session = new JT808UdpSession(socket, remoteEndPoint);
                session.TerminalPhoneNo = terminalPhoneNo;
                Sessions.TryAdd(session.SessionID, session);
                TerminalPhoneNoSessions.TryAdd(terminalPhoneNo, session);
                currentSession = session;
            }
            //会话通知
            //使用场景：
            //部标的超长待机设备,不会像正常的设备一样一直连着，可能10几分钟连上了，然后发完就关闭连接，
            //这时候想下发数据需要知道设备什么时候上线，在这边做通知最好不过了。
            //有设备关联上来可以进行通知 例如：使用Redis发布订阅
            if (SessionProducer != null)
            {
                SessionProducer.ProduceAsync(JT808GatewayConstants.SessionOnline, terminalPhoneNo);
            }
            return currentSession;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        internal bool TryAdd(IJT808Session session)
        {
            return Sessions.TryAdd(session.SessionID, session);
        }

        public async ValueTask<bool> TrySendByTerminalPhoneNoAsync(string terminalPhoneNo, byte[] data)
        {
            if (TerminalPhoneNoSessions.TryGetValue(terminalPhoneNo, out var session))
            {
                if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                {
                    await session.Client.SendAsync(data, SocketFlags.None);
                }
                else
                {
                    await session.Client.SendToAsync(data, SocketFlags.None, session.RemoteEndPoint);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async ValueTask<bool> TrySendBySessionIdAsync(string sessionId, byte[] data)
        {
            if (Sessions.TryGetValue(sessionId, out var session))
            {
                if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                {
                    await session.Client.SendAsync(data, SocketFlags.None);
                }
                else
                {
                    await session.Client.SendToAsync(data, SocketFlags.None, session.RemoteEndPoint);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveByTerminalPhoneNo(string terminalPhoneNo)
        {
            if (TerminalPhoneNoSessions.TryGetValue(terminalPhoneNo, out var removeTerminalPhoneNoSessions))
            {
                // 处理转发过来的是数据 这时候通道对设备是1对多关系,需要清理垃圾数据
                //1.用当前会话的通道Id找出通过转发过来的其他设备的终端号
                var terminalPhoneNos = TerminalPhoneNoSessions.Where(w => w.Value.SessionID == removeTerminalPhoneNoSessions.SessionID).Select(s => s.Key).ToList();
                //2.存在则一个个移除 
                string tmpTerminalPhoneNo = terminalPhoneNo;
                if (terminalPhoneNos.Count > 0)
                {
                    //3.移除包括当前的设备号
                    foreach (var item in terminalPhoneNos)
                    {
                        TerminalPhoneNoSessions.TryRemove(item, out _);
                    }
                    tmpTerminalPhoneNo = string.Join(",", terminalPhoneNos);
                }
                if (Sessions.TryRemove(removeTerminalPhoneNoSessions.SessionID, out var removeSession))
                {
                    removeSession.Close();
                    if (logger.IsEnabled(LogLevel.Information))
                        logger.LogInformation($"[Session Remove]:{terminalPhoneNo}-{tmpTerminalPhoneNo}");
                    if (SessionProducer != null)
                    {
                        SessionProducer.ProduceAsync(JT808GatewayConstants.SessionOffline, tmpTerminalPhoneNo);
                    }
                }
            }
        }

        public void RemoveBySessionId(string sessionId)
        {
            if (Sessions.TryRemove(sessionId, out var removeSession))
            {
                var terminalPhoneNos = TerminalPhoneNoSessions.Where(w => w.Value.SessionID == sessionId).Select(s => s.Key).ToList();
                if (terminalPhoneNos.Count > 0)
                {
                    foreach (var item in terminalPhoneNos)
                    {
                        TerminalPhoneNoSessions.TryRemove(item, out _);
                    }
                    var tmpTerminalPhoneNo = string.Join(",", terminalPhoneNos);
                    if (SessionProducer != null)
                    {
                        SessionProducer.ProduceAsync(JT808GatewayConstants.SessionOffline, tmpTerminalPhoneNo);
                    }
                    if (logger.IsEnabled(LogLevel.Information))
                        logger.LogInformation($"[Session Remove]:{tmpTerminalPhoneNo}");
                }
                removeSession.Close();
            }
        }

        public List<JT808TcpSession> GetTcpAll(Func<IJT808Session, bool> predicate = null)
        {
            var query = TerminalPhoneNoSessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.tcp);
            if (predicate != null)
            {
                query = query.Where(s => predicate(s.Value));
            }
            return query.Select(s => (JT808TcpSession)s.Value).ToList();
        }

        public IEnumerable<JT808TcpSession> GetTcpByPage(Func<IJT808Session, bool> predicate = null)
        {
            var query = TerminalPhoneNoSessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.tcp);
            if (predicate != null)
            {
                query = query.Where(s => predicate(s.Value));
            }
            return query.Select(s => (JT808TcpSession)s.Value);
        }


        public List<JT808UdpSession> GetUdpAll(Func<IJT808Session, bool> predicate = null)
        {
            var query = TerminalPhoneNoSessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.udp);
            if (predicate != null)
            {
                query = query.Where(s => predicate(s.Value));
            }
            return query.Select(s => (JT808UdpSession)s.Value).ToList();
        }

        public IEnumerable<JT808UdpSession> GetUdpByPage(Func<IJT808Session, bool> predicate = null)
        {
            var query = TerminalPhoneNoSessions.Where(w => w.Value.TransportProtocolType == JT808TransportProtocolType.udp);
            if (predicate != null)
            {
                query = query.Where(s => predicate(s.Value));
            }
            return query.Select(s => (JT808UdpSession)s.Value);
        }
    }
}
