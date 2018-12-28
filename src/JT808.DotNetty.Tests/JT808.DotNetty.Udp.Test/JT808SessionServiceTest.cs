using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using JT808.Protocol.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JT808.DotNetty.Udp.Test
{
    [TestClass]
    public class JT808SessionServiceTest:TestBase,IDisposable
    {
        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 818);
        JT808SimpleUdpClient SimpleUdpClient1;
        JT808SimpleUdpClient SimpleUdpClient2;
        JT808SimpleUdpClient SimpleUdpClient3;
        JT808SimpleUdpClient SimpleUdpClient4;
        JT808SimpleUdpClient SimpleUdpClient5;

        public void Dispose()
        {
            SimpleUdpClient1.Down();
            SimpleUdpClient2.Down();
            SimpleUdpClient3.Down();
            SimpleUdpClient4.Down();
            SimpleUdpClient5.Down();
        }

        public JT808SessionServiceTest()
        {
            SimpleUdpClient1 = new JT808SimpleUdpClient(endPoint);
            SimpleUdpClient2 = new JT808SimpleUdpClient(endPoint);
            SimpleUdpClient3 = new JT808SimpleUdpClient(endPoint);
            SimpleUdpClient4 = new JT808SimpleUdpClient(endPoint);
            SimpleUdpClient5 = new JT808SimpleUdpClient(endPoint);
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789001");
            SimpleUdpClient1.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            // 心跳会话包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789002");
            SimpleUdpClient2.WriteAsync(JT808Serializer.Serialize(jT808Package2));

            // 心跳会话包
            JT808Package jT808Package3 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789003");
            SimpleUdpClient3.WriteAsync(JT808Serializer.Serialize(jT808Package3));

            // 心跳会话包
            JT808Package jT808Package4 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789004");
            SimpleUdpClient4.WriteAsync(JT808Serializer.Serialize(jT808Package4));

            // 心跳会话包
            JT808Package jT808Package5 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789005");
            SimpleUdpClient5.WriteAsync(JT808Serializer.Serialize(jT808Package5));
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void Test1()
        {
            IJT808UdpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808UdpSessionService>();
            var result = jT808SessionServiceDefaultImpl.GetAll();
        }

        [TestMethod]
        public void Test2()
        {
            IJT808UdpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808UdpSessionService>();
            var result1 = jT808SessionServiceDefaultImpl.GetAll();
            var result2 = jT808SessionServiceDefaultImpl.RemoveByTerminalPhoneNo("123456789001");
            var result3 = jT808SessionServiceDefaultImpl.GetAll();
        }

        [TestMethod]
        public void Test3()
        {
            // 判断通道是否关闭
            IJT808UdpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808UdpSessionService>();
            JT808UdpSessionManager jT808UdpSessionManager = ServiceProvider.GetService<JT808UdpSessionManager>();
            var result1 = jT808SessionServiceDefaultImpl.GetAll();
            SimpleUdpClient1.Down();
            var session = jT808UdpSessionManager.GetSession("123456789001");
            var result3 = jT808UdpSessionManager.GetAll();
            Thread.Sleep(100000);
        }
    }
}
