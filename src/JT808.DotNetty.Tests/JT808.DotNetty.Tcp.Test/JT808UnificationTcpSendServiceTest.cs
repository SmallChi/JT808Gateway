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
using JT808.Protocol.MessageBody;
using JT808.DotNetty.Abstractions.Dtos;

namespace JT808.DotNetty.Tcp.Test
{
    [TestClass]
    public class JT808UnificationTcpSendServiceTest: TestBase
    {
        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        private IJT808UnificationTcpSendService jT808UnificationTcpSendService;
        private IJT808TcpSessionService jT808SessionServiceDefaultImpl;

        public JT808UnificationTcpSendServiceTest()
        {
            
            JT808SimpleTcpClient SimpleTcpClient1 = new JT808SimpleTcpClient(endPoint);
            JT808SimpleTcpClient SimpleTcpClient2 = new JT808SimpleTcpClient(endPoint);
            JT808SimpleTcpClient SimpleTcpClient3 = new JT808SimpleTcpClient(endPoint);
            JT808SimpleTcpClient SimpleTcpClient4 = new JT808SimpleTcpClient(endPoint);
            JT808SimpleTcpClient SimpleTcpClient5 = new JT808SimpleTcpClient(endPoint);
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

            Thread.Sleep(300);
        }

        [TestMethod]
        public void Test1()
        {
            jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808TcpSessionService>();
            jT808UnificationTcpSendService = ServiceProvider.GetService<IJT808UnificationTcpSendService>();
            jT808SessionServiceDefaultImpl.GetAll();
            string no = "123456789001";
            // 文本信息包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.文本信息下发.Create(no, new JT808_0x8300
            {
                TextFlag = 5,
                TextInfo = "smallchi 518"
            });
            var data = JT808Serializer.Serialize(jT808Package2);
            JT808ResultDto<bool> jt808Result = jT808UnificationTcpSendService.Send(no, data);
            Thread.Sleep(1000);
            Assert.AreEqual(200, jt808Result.Code);
            Assert.IsTrue(jt808Result.Data);
        }
    }
}
