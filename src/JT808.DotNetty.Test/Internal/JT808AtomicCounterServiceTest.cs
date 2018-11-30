using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Codecs;
using JT808.DotNetty.Dtos;
using JT808.DotNetty.Metadata;
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
using System.Threading.Tasks;
using Xunit;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808AtomicCounterServiceTest: TestBase
    {
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        public JT808SimpleTcpClient SimpleTcpClient;

        public JT808AtomicCounterServiceTest()
        {
            SimpleTcpClient = new JT808SimpleTcpClient(endPoint);
        }

        [Fact]
        public void Test1()
        {
            JT808AtomicCounter jT808AtomicCounter = new JT808AtomicCounter();
            JT808AtomicCounter jT808AtomicCounter1 = new JT808AtomicCounter();
            Parallel.For(0, 1000, (i) => 
            {
                jT808AtomicCounter.Increment();
            });
            Assert.Equal(1000, jT808AtomicCounter.Count);
            Parallel.For(0, 1000, (i) =>
            {
                jT808AtomicCounter1.Increment();
            });
            Assert.Equal(1000, jT808AtomicCounter1.Count);
        }

        [Fact]
        public void Test2()
        {
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789001");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            // 心跳会话包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789002");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package2));

            // 心跳会话包
            JT808Package jT808Package3 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789003");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package3));

            // 心跳会话包
            JT808Package jT808Package4 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789004");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package4));

            // 心跳会话包
            JT808Package jT808Package5 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789005");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package5));

            // 异步的需要延时下
            Thread.Sleep(1000);

            HttpClient httpClient = new HttpClient();
            // 调用内置的http服务接收文本信息下发
            var result = httpClient.GetAsync("http://127.0.0.1:828/jt808api/GetAtomicCounter").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<JT808AtomicCounterDto> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<JT808AtomicCounterDto>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Equal(5,jt808Result.Data.MsgSuccessCount);
            Assert.Equal(0, jt808Result.Data.MsgFailCount);
            SimpleTcpClient.Down();
        }
    }
}
