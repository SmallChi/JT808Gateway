using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Core.Impls;
using System.Threading;
using Xunit;

namespace JT808.DotNetty.Core.Test
{
    public class JT808SessionManagerTest: SeedTcpSession
    {
        [Fact]
        public void AddTest()
        {
            var no = "test150";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no,channel);
            jT80TcpSessionManager.Heartbeat(no);
            Assert.NotNull(jT80TcpSessionManager.GetTcpSessionByTerminalPhoneNo(no));
        }

        [Fact]
        public void RemoveTest()
        {
            var no = "test151";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT80TcpSessionManager.TryAdd(no, channel);
            var sessionInfo = jT80TcpSessionManager.RemoveSession(no);
            Assert.Equal(no, sessionInfo.TerminalPhoneNo);
        }

        [Fact]
        public void OneChannelToManyDeviceTest1()
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
            Assert.Equal(no, removeSession.TerminalPhoneNo);
            Assert.Equal(channel, removeSession.Channel);
            Assert.Equal(1,channel.Id.CompareTo(removeSession.Channel.Id));
        }


        [Fact]
        public void OneChannelToManyDeviceTest2()
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
            Assert.Null(jT80TcpSessionManager.GetTcpSessionByTerminalPhoneNo(no));
            Assert.Null(jT80TcpSessionManager.GetTcpSessionByTerminalPhoneNo(no1));
            Assert.Null(jT80TcpSessionManager.GetTcpSessionByTerminalPhoneNo(no2));
        }
    }
}
