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

namespace JT808.DotNetty.Udp.Test
{
    [TestClass]
    public class JT808UnificationUdpSendServiceTest : TestBase
    {
        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 818);

        private IJT808UnificationSendService jT808UnificationSendService;
        private IJT808SessionService jT808SessionServiceDefaultImpl;

        public JT808UnificationUdpSendServiceTest()
        {    
            JT808SimpleUdpClient SimpleUdpClient1 = new JT808SimpleUdpClient(endPoint);
            JT808SimpleUdpClient SimpleUdpClient2 = new JT808SimpleUdpClient(endPoint);
            JT808SimpleUdpClient SimpleUdpClient3 = new JT808SimpleUdpClient(endPoint);
            JT808SimpleUdpClient SimpleUdpClient4 = new JT808SimpleUdpClient(endPoint);
            JT808SimpleUdpClient SimpleUdpClient5 = new JT808SimpleUdpClient(endPoint);
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

            Thread.Sleep(300);
        }

        [TestMethod]
        public void Test1()
        {
            //"126 131 0 0 13 18 52 86 120 144 1 0 11 5 115 109 97 108 108 99 104 105 32 53 49 56 24 126"
            jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808SessionService>();
            jT808UnificationSendService = ServiceProvider.GetService<IJT808UnificationSendService>();
            jT808SessionServiceDefaultImpl.GetUdpAll();
            string no = "123456789001";
            // 文本信息包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.文本信息下发.Create(no, new JT808_0x8300
            {
                TextFlag = 5,
                TextInfo = "smallchi 518"
            });
            var data = JT808Serializer.Serialize(jT808Package2);
            JT808ResultDto<bool> jt808Result = jT808UnificationSendService.Send(no, data);
            Thread.Sleep(1000);
            Assert.AreEqual(200, jt808Result.Code);
            Assert.IsTrue(jt808Result.Data);
        }
    }
}
