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
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Abstractions.Dtos;
using Xunit;
using DotNetty.Transport.Channels.Embedded;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Udp.Handlers;
using JT808.Protocol.MessageBody;
using System.Linq;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Buffers;

namespace JT808.DotNetty.Udp.Test
{
    public class JT808SessionServiceTest:TestBase
    {
        List<string> TNos = new List<string> {
                "123456789001",
                "123456789002",
                "123456789003",
                "123456789004",
                "123456789005"
        };
        public JT808SessionServiceTest()
        {
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789001");
            var ch1 = CreateEmbeddedChannel();
            ch1.WriteInbound(JT808Serializer.Serialize(jT808Package1));
            SeedSession(TNos.ToArray());
        }

        [Fact]
        public void GetUdpAllTest()
        {
            IJT808SessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808SessionService>();
            var result = jT808SessionServiceDefaultImpl.GetUdpAll();
            var tons = result.Data.Select(s => s.TerminalPhoneNo).ToList();
            foreach (var item in TNos)
            {
                Assert.Contains(item, tons);
            }
            Assert.Equal(JT808ResultCode.Ok, result.Code);
        }

        [Fact]
        public void RemoveByTerminalPhoneNoTest()
        {
            string tno = "123456789006";
            IJT808SessionService jT808SessionServiceDefaultImpl = ServiceProvider.GetService<IJT808SessionService>();
            SeedSession(tno);
            var result1 = jT808SessionServiceDefaultImpl.RemoveByTerminalPhoneNo(tno);
            Assert.Equal(JT808ResultCode.Ok, result1.Code);
            Assert.True(result1.Data);
            var result2 = jT808SessionServiceDefaultImpl.GetUdpAll();
            Assert.Equal(JT808ResultCode.Ok, result2.Code);
            Assert.DoesNotContain(tno, result2.Data.Select(s=>s.TerminalPhoneNo));
        }

        [Fact]
        public void SendTest()
        {
            //"126 131 0 0 13 18 52 86 120 144 1 0 11 5 115 109 97 108 108 99 104 105 32 53 49 56 24 126"
            var jT808UnificationSendService = ServiceProvider.GetService<IJT808UnificationSendService>();
            string no = "123456789001";
            // 文本信息包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.文本信息下发.Create(no, new JT808_0x8300
            {
                TextFlag = 5,
                TextInfo = "smallchi 518"
            });
            var data = JT808Serializer.Serialize(jT808Package2);
            JT808ResultDto<bool> jt808Result = jT808UnificationSendService.Send(no, data);
            Assert.Equal(JT808ResultCode.Ok, jt808Result.Code);
            Assert.True(jt808Result.Data);
            if(Channels.TryGetValue(no,out var channel))
            {
                var package = channel.ReadOutbound<DatagramPacket>();
                byte[] recevie = new byte[package.Content.Capacity];
                package.Content.ReadBytes(recevie);
                Assert.Equal(data, recevie);
            }
        }
    }
}
