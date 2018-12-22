using DotNetty.Transport.Channels.Embedded;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class JT808SessionManagerTest: SeedSession
    {
        [Fact]
        public void Test1()
        {
            var no = "test150";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no));
            Thread.Sleep(1000);
            jT808SessionManager.Heartbeat(no);
        }

        [Fact]
        public void Test2()
        {
            var no = "test151";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no));
            var sessionInfo = jT808SessionManager.RemoveSession(no);
            Assert.Equal(no, sessionInfo.TerminalPhoneNo);
        }

        [Fact]
        public void Test3()
        {
            var realSessionInfos = jT808SessionManager.GetAll();
        }

        [Fact]
        public void Test4()
        {
            var realSessionCount = jT808SessionManager.SessionCount;
        }

        [Fact]
        public void Test5()
        {
            //转发过来的数据 1:n 一个通道对应多个设备
            var no = "test1";
            var no1 = "test2";
            var no2 = "test3";
            var no3 = "test4";
            var no4 = "test5";
            var channel = new EmbeddedChannel(new JT808DefaultChannelId());
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no1));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no2));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no3));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, no4));
            var removeSession = jT808SessionManager.RemoveSession(no);
            Assert.Equal(no, removeSession.TerminalPhoneNo);
            Assert.Equal(channel, removeSession.Channel);
            Assert.Equal(channel.Id, removeSession.Channel.Id);
        }

        [Fact]
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
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel1, no));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel1, no1));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel1, no2));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel2, no3));
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel2, no4));
            jT808SessionManager.RemoveSessionByChannel(channel1);
        }
    }
}
