using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Codecs;
using JT808.DotNetty.Dtos;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Xunit;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808UnificationSendServiceDefaultImplTest : TestBase
    {
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        public JT808SimpleTcpClient SimpleTcpClient;

        public JT808UnificationSendServiceDefaultImplTest()
        {
            SimpleTcpClient = new JT808SimpleTcpClient(endPoint);
        }

        [Fact]
        public void Test1()
        {
            // 文本信息包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.文本信息下发.Create("123456789012", new JT808_0x8300
            {
                TextFlag = 5,
                TextInfo = "smallchi 518"
            });
            var data = JT808Serializer.Serialize(jT808Package2);

            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = new JT808UnificationSendRequestDto();
            jT808UnificationSendRequestDto.TerminalPhoneNo = "123456789012";
            jT808UnificationSendRequestDto.Data = data;

            HttpClient httpClient = new HttpClient();
            // 调用内置的http服务接收文本信息下发
            var result = httpClient.PostAsync("http://127.0.0.1:828/jt808api/UnificationSend", new StringContent(JsonConvert.SerializeObject(jT808UnificationSendRequestDto))).Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.False(jt808Result.Data);
        }

        [Fact]
        public void Test2()
        {
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789012");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            // 文本信息包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.文本信息下发.Create("123456789012", new JT808_0x8300
            {
                  TextFlag=5,
                  TextInfo= "smallchi 518"
            });
            var data = JT808Serializer.Serialize(jT808Package2);

            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = new JT808UnificationSendRequestDto();
            jT808UnificationSendRequestDto.TerminalPhoneNo = "123456789012";
            jT808UnificationSendRequestDto.Data = data;

            HttpClient httpClient = new HttpClient();
            // 调用内置的http服务接收文本信息下发
            var result =httpClient.PostAsync("http://127.0.0.1:828/jt808api/UnificationSend", new StringContent(JsonConvert.SerializeObject(jT808UnificationSendRequestDto))).Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.True(jt808Result.Data);
            SimpleTcpClient.Down();
        }
    }
}
