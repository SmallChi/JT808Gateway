using DotNetty.Transport.Channels.Embedded;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class JT808SessionManagerTest: SeedSession
    {
        string TerminalPhoneNo = "123456789123";

        [Fact]
        public void Test3()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, TerminalPhoneNo));
            var sessionInfo=jT808SessionManager.GetSession(TerminalPhoneNo);
            Assert.Equal(TerminalPhoneNo, sessionInfo.TerminalPhoneNo);
            Assert.Equal("123456789123", sessionInfo.TerminalPhoneNo);
        }

        [Fact]
        public void Test4()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, TerminalPhoneNo));
            jT808SessionManager.Heartbeat(TerminalPhoneNo);
        }

        [Fact]
        public void Test5()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, TerminalPhoneNo));
            var sessionInfo = jT808SessionManager.GetSession(TerminalPhoneNo);
            Assert.Equal(TerminalPhoneNo, sessionInfo.TerminalPhoneNo);
        }


        [Fact]
        public void Test6()
        {
            var channel = new EmbeddedChannel();
            jT808SessionManager.TryAdd(new Metadata.JT808Session(channel, TerminalPhoneNo));
            var sessionInfo = jT808SessionManager.RemoveSession(TerminalPhoneNo);
            Assert.Equal(TerminalPhoneNo, sessionInfo.TerminalPhoneNo);
        }


        //[Fact]
        //public void Test7()
        //{
        //    var channel = new EmbeddedChannel();
        //    var sessionInfo = jT808SessionManager.RemoveSessionByTerminalPhoneNo(TerminalPhoneNo);
        //    Assert.Equal(TerminalPhoneNo, sessionInfo.TerminalPhoneNo);
        //    Assert.Equal("embedded", sessionInfo.SessionID);
        //}

        [Fact]
        public void Test8()
        {
            var realSessionInfos = jT808SessionManager.GetAll();
        }

        [Fact]
        public void Test9()
        {
            var realSessionCount = jT808SessionManager.SessionCount;
        }
    }
}
