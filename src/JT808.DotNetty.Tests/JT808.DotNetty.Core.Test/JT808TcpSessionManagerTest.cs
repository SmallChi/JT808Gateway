using DotNetty.Transport.Channels.Embedded;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;


namespace JT808.DotNetty.Core.Test
{
    [TestClass]
    public class JT808SessionManagerTest: SeedTcpSession
    {
        [TestMethod]
        public void Test1()
        {
            var no = "test150";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no,channel);
            Thread.Sleep(1000);
            jT80TcpSessionManager.Heartbeat(no);
        }

        [TestMethod]
        public void Test2()
        {
            var no = "test151";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no, channel);
            var sessionInfo = jT80TcpSessionManager.RemoveSession(no);
            Assert.AreEqual(no, sessionInfo.TerminalPhoneNo);
        }

        [TestMethod]
        public void Test3()
        {
            var realSessionInfos = jT80TcpSessionManager.GetAll();
        }

        [TestMethod]
        public void Test4()
        {
            var realSessionCount = jT80TcpSessionManager.SessionCount;
        }

        [TestMethod]
        public void Test5()
        {
            //转发过来的数据 1:n 一个通道对应多个设备
            var no = "test1";
            var no1 = "test2";
            var no2 = "test3";
            var no3 = "test4";
            var no4 = "test5";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no,channel);
            jT80TcpSessionManager.TryAdd(no1,channel);
            jT80TcpSessionManager.TryAdd(no2,channel);
            jT80TcpSessionManager.TryAdd(no3,channel);
            jT80TcpSessionManager.TryAdd(no4,channel);
            var removeSession = jT80TcpSessionManager.RemoveSession(no);
            Assert.AreEqual(no, removeSession.TerminalPhoneNo);
            Assert.AreEqual(channel, removeSession.Channel);
            Assert.AreEqual(channel.Id, removeSession.Channel.Id);
        }

        [TestMethod]
        public void Test6()
        {
            //转发过来的数据 1:n 一个通道对应多个设备
            var no = "test61";
            var no1 = "test62";
            var no2 = "test63";
            var no3 = "test64";
            var no4 = "test65";
            var channel1 = new EmbeddedChannel(new JT808DefaultChannelId());
            var channel2 = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no,channel1);
            jT80TcpSessionManager.TryAdd(no1,channel1);
            jT80TcpSessionManager.TryAdd(no2,channel1);
            jT80TcpSessionManager.TryAdd(no3,channel2);
            jT80TcpSessionManager.TryAdd(no4,channel2);
            jT80TcpSessionManager.RemoveSessionByChannel(channel1);
        }
    }
}
