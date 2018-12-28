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

namespace JT808.DotNetty.Tcp.Test
{
    [TestClass]
    public class JT808SessionServiceTest:TestBase,IDisposable
    {
        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        JT808SimpleTcpClient SimpleTcpClient1;
        JT808SimpleTcpClient SimpleTcpClient2;
        JT808SimpleTcpClient SimpleTcpClient3;
        JT808SimpleTcpClient SimpleTcpClient4;
        JT808SimpleTcpClient SimpleTcpClient5;

        public JT808SessionServiceTest()
        {
            SimpleTcpClient1 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient2 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient3 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient4 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient5 = new JT808SimpleTcpClient(endPoint);
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789001");
            SimpleTcpClient1.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            // 心跳会话包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789002");
            SimpleTcpClient2.WriteAsync(JT808Serializer.Serialize(jT808Package2));

            // 心跳会话包
            JT808Package jT808Package3 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789003");
            SimpleTcpClient3.WriteAsync(JT808Serializer.Serialize(jT808Package3));

            // 心跳会话包
            JT808Package jT808Package4 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789004");
            SimpleTcpClient4.WriteAsync(JT808Serializer.Serialize(jT808Package4));

            // 心跳会话包
            JT808Package jT808Package5 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789005");
            SimpleTcpClient5.WriteAsync(JT808Serializer.Serialize(jT808Package5));
            Thread.Sleep(1000);
        }

        public void Dispose()
        {
            SimpleTcpClient1.Down();
            SimpleTcpClient2.Down();
            SimpleTcpClient3.Down();
            SimpleTcpClient4.Down();
            SimpleTcpClient5.Down();
        }

        [TestMethod]
        public void Test1()
        {
            IJT808TcpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808TcpSessionService>();
            var result = jT808SessionServiceDefaultImpl.GetAll();
            Thread.Sleep(5000);
        }

        [TestMethod]
        public void Test2()
        {
            IJT808TcpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808TcpSessionService>();
            var result1 = jT808SessionServiceDefaultImpl.GetAll();
            var result2 = jT808SessionServiceDefaultImpl.RemoveByTerminalPhoneNo("123456789001");
            var result3 = jT808SessionServiceDefaultImpl.GetAll();
        }

        [TestMethod]
        public void Test3()
        {
            // 判断通道是否关闭
            IJT808TcpSessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808TcpSessionService>();
            JT808TcpSessionManager jT808TcpSessionManager = ServiceProvider.GetService<JT808TcpSessionManager>();
            var result1 = jT808SessionServiceDefaultImpl.GetAll();
            SimpleTcpClient1.Down();
            Thread.Sleep(5000);
            var session = jT808TcpSessionManager.GetSession("123456789001");
            Thread.Sleep(100000);
        }
    }
}
