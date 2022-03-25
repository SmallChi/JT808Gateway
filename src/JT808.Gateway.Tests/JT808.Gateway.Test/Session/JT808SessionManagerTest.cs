using JT808.Gateway.Session;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace JT808.Gateway.Test.Session
{
    public class JT808SessionManagerTest
    {
        [Fact]
        public void TryAddTest()
        {
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var result=jT808SessionManager.TryAdd(new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp)));
            Assert.True(result);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
        }

        [Fact]
        public void TryLinkTest()
        {
            string tno = "123456";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session);
            jT808SessionManager.TryLink(tno, session);
            Assert.True(result1);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
            Assert.True(jT808SessionManager.TerminalPhoneNoSessions.ContainsKey(tno));
        }

        /// <summary>
        /// 用于转发过来的车辆
        /// </summary>
        [Fact]
        public void TryLinkTest1_1_N()
        {
            string tno1 = "123456";
            string tno2 = "123457";
            string tno3 = "123458";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session);
            jT808SessionManager.TryLink(tno1, session);
            jT808SessionManager.TryLink(tno2, session);
            jT808SessionManager.TryLink(tno3, session);
            Assert.True(result1);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
            Assert.Equal(3,jT808SessionManager.TerminalPhoneNoSessions.Count);
            jT808SessionManager.RemoveBySessionId(session.SessionID);
            Assert.Equal(0, jT808SessionManager.TotalSessionCount);
            Assert.Empty(jT808SessionManager.TerminalPhoneNoSessions);
        }

        /// <summary>
        /// 用于转发过来的车辆
        /// </summary>
        [Fact]
        public void TryLinkTest2_1_N()
        {
            string tno1 = "123456";
            string tno2 = "123457";
            string tno3 = "123458";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session);
            jT808SessionManager.TryLink(tno1, session);
            jT808SessionManager.TryLink(tno2, session);
            jT808SessionManager.TryLink(tno3, session);
            Assert.True(result1);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
            Assert.Equal(3, jT808SessionManager.TerminalPhoneNoSessions.Count);
            jT808SessionManager.RemoveByTerminalPhoneNo(tno1);
            Assert.Equal(0, jT808SessionManager.TotalSessionCount);
            Assert.Empty(jT808SessionManager.TerminalPhoneNoSessions);
        }

        /// <summary>
        /// 转发过来的车辆切换为直连车辆
        /// </summary>
        [Fact]
        public void UpdateLinkTest2_1_N()
        {
            string tno1 = "123456";
            string tno2 = "123457";
            string tno3 = "123458";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session1 = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var session2 = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session1);
            var result2 = jT808SessionManager.TryAdd(session2);
            //转发车辆
            jT808SessionManager.TryLink(tno1, session1);
            jT808SessionManager.TryLink(tno2, session1);
            //直连车辆
            jT808SessionManager.TryLink(tno3, session2);

            Assert.True(result1);
            Assert.True(result2);
            Assert.Equal(2, jT808SessionManager.TotalSessionCount);
            Assert.Equal(3, jT808SessionManager.TerminalPhoneNoSessions.Count);

            //将tno2切换为直连车辆
            var session3 = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result3 = jT808SessionManager.TryAdd(session3);
            jT808SessionManager.TryLink(tno2, session3);
            Assert.True(result3);
            if (jT808SessionManager.TerminalPhoneNoSessions.TryGetValue(tno2,out var sessionInfo))
            {
                //实际的通道Id
                Assert.Equal(session3.SessionID, sessionInfo.SessionID);
            }
            Assert.Equal(3, jT808SessionManager.TotalSessionCount);
            Assert.Equal(3, jT808SessionManager.TerminalPhoneNoSessions.Count);

            jT808SessionManager.RemoveByTerminalPhoneNo(tno1);
            Assert.Equal(2, jT808SessionManager.TotalSessionCount);
            Assert.Equal(2,jT808SessionManager.TerminalPhoneNoSessions.Count);
        }

        [Fact]
        public void RemoveBySessionIdTest()
        {
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session);
            Assert.True(result1);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
            jT808SessionManager.RemoveBySessionId(session.SessionID);
            Assert.Equal(0, jT808SessionManager.TotalSessionCount);
        }

        [Fact]
        public void RemoveByTerminalPhoneNoTest()
        {
            string tno = "123456";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session);
            jT808SessionManager.TryLink(tno, session);
            Assert.True(result1);
            Assert.Equal(1, jT808SessionManager.TotalSessionCount);
            jT808SessionManager.RemoveByTerminalPhoneNo(tno);
            Assert.False(jT808SessionManager.TerminalPhoneNoSessions.ContainsKey(tno));
            Assert.Equal(0, jT808SessionManager.TotalSessionCount);
        }

        [Fact]
        public void SendTest()
        {
            Assert.ThrowsAsync<SocketException>(async() => 
            {
                string tno = "123456";
                JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
                var session = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
                var result1 = jT808SessionManager.TryAdd(session);
                jT808SessionManager.TryLink(tno, session);
                await jT808SessionManager.TrySendByTerminalPhoneNoAsync(tno, new byte[] { 0x7e, 0, 0, 0x7e });
            });
        }

        [Fact]
        public void GetTcpAllTest()
        {
            string tno1 = "123456";
            string tno2 = "123457";
            JT808SessionManager jT808SessionManager = new JT808SessionManager(new LoggerFactory());
            var session1 = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var session2 = new JT808TcpSession(new Socket(SocketType.Stream, ProtocolType.Tcp));
            var result1 = jT808SessionManager.TryAdd(session1);
            var result2 = jT808SessionManager.TryAdd(session2);
            jT808SessionManager.TryLink(tno1, session1);
            jT808SessionManager.TryLink(tno2, session2);
            Assert.True(result1);
            Assert.True(result2);
            Assert.Equal(2, jT808SessionManager.TotalSessionCount);
            Assert.True(jT808SessionManager.TerminalPhoneNoSessions.ContainsKey(tno1));
            Assert.True(jT808SessionManager.TerminalPhoneNoSessions.ContainsKey(tno2));
            var sessions = jT808SessionManager.GetTcpAll();
            Assert.Contains(sessions, (item) => item.SessionID == session1.SessionID);
            Assert.Contains(sessions, (item) => item.SessionID == session2.SessionID);
        }
    }
}
