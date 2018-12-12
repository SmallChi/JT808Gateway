using JT808.DotNetty.Dtos;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class JT808WebAPIServiceTest: TestBase
    {
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        public JT808SimpleTcpClient SimpleTcpClient;

        private HttpClient httpClient;

        private const string Url = "http://127.0.0.1:828/jt808api";

        private const string sessionRoutePrefix = "Session";

        private const int length = 10;

        public JT808WebAPIServiceTest()
        {
            SimpleTcpClient = new JT808SimpleTcpClient(endPoint);
            httpClient = new HttpClient();
            for (var i = 1; i <= length; i++)
            {
                // 心跳会话包
                JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create($"{i.ToString()}");
                SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));
            }
            Thread.Sleep(300);
        }

        [Fact]
        public void GetAllTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetAll").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<IEnumerable<JT808SessionInfoDto>> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<IEnumerable<JT808SessionInfoDto>>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Equal(10,jt808Result.Data.Count());
        }

        [Fact]
        public void RemoveByChannelIdTest()
        {
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("99");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            var result4 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/RemoveByChannelId", new StringContent("99")).Result;
            string content4 = result4.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result4= JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content4);
            Assert.Equal(200, jt808Result4.Code);
            Assert.True(jt808Result4.Data);
        }


        [Fact]
        public void RemoveByTerminalPhoneNoTest()
        {
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("999");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));


            var result4 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/RemoveByTerminalPhoneNo", new StringContent("999")).Result;
            string content4 = result4.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result4 = JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content4);
            Assert.Equal(200, jt808Result4.Code);
            Assert.True(jt808Result4.Data);
        }


        public override void Dispose()
        {
            base.Dispose();
            SimpleTcpClient.Down();
        }
    }
}
