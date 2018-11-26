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
        public void GetRealLinkCountTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRealLinkCount").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Equal(1,jt808Result.Data);
        }

        [Fact]
        public void GetRelevanceLinkCountTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Equal(length, jt808Result.Data);
        }

        [Fact]
        public void GetRealAllTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRealAll").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<IEnumerable<JT808SessionInfoDto>> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<IEnumerable<JT808SessionInfoDto>>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Single(jt808Result.Data);
        }

        [Fact]
        public void GetRelevanceAllTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceAll").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<IEnumerable<JT808SessionInfoDto>> jt808Result = JsonConvert.DeserializeObject<JT808ResultDto<IEnumerable<JT808SessionInfoDto>>>(content);
            Assert.Equal(200, jt808Result.Code);
            Assert.Equal(length,jt808Result.Data.Count());
        }

        [Fact]
        public void RemoveByChannelIdTest()
        {
            var result1 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content1 = result1.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result1 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content1);
            Assert.Equal(200, jt808Result1.Code);
            Assert.Equal(length, jt808Result1.Data);

            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("99");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            var result2 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content2 = result2.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result2 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content2);
            Assert.Equal(200, jt808Result2.Code);
            Assert.Equal(length + 1, jt808Result2.Data);

            var result3 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/GetByTerminalPhoneNo", new StringContent("99")).Result;
            string content3 = result3.Content.ReadAsStringAsync().Result;

            JT808ResultDto<JT808SessionInfoDto> jt808Result3 = JsonConvert.DeserializeObject<JT808ResultDto<JT808SessionInfoDto>>(content3);
            Assert.Equal(200, jt808Result3.Code);
            Assert.Equal("99", jt808Result3.Data.TerminalPhoneNo);

            var result4 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/RemoveByChannelId", new StringContent(jt808Result3.Data.ChannelId)).Result;
            string content4 = result4.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result4= JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content4);
            Assert.Equal(200, jt808Result4.Code);
            Assert.True(jt808Result4.Data);

            var result5 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content5 = result5.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result5 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content5);
            Assert.Equal(200, jt808Result5.Code);
            Assert.Equal(length, jt808Result5.Data);
        }


        [Fact]
        public void RemoveByTerminalPhoneNoTest()
        {
            var result1 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content1 = result1.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result1 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content1);
            Assert.Equal(200, jt808Result1.Code);
            Assert.Equal(length, jt808Result1.Data);

            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("999");
            SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            var result2 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content2 = result2.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result2 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content2);
            Assert.Equal(200, jt808Result2.Code);
            Assert.Equal(length + 1, jt808Result2.Data);

            var result3 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/GetByTerminalPhoneNo", new StringContent("999")).Result;
            string content3 = result3.Content.ReadAsStringAsync().Result;

            JT808ResultDto<JT808SessionInfoDto> jt808Result3 = JsonConvert.DeserializeObject<JT808ResultDto<JT808SessionInfoDto>>(content3);
            Assert.Equal(200, jt808Result3.Code);
            Assert.Equal("999", jt808Result3.Data.TerminalPhoneNo);

            var result4 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/RemoveByTerminalPhoneNo", new StringContent("999")).Result;
            string content4 = result4.Content.ReadAsStringAsync().Result;
            JT808ResultDto<bool> jt808Result4 = JsonConvert.DeserializeObject<JT808ResultDto<bool>>(content4);
            Assert.Equal(200, jt808Result4.Code);
            Assert.True(jt808Result4.Data);

            var result5 = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceLinkCount").Result;
            string content5 = result5.Content.ReadAsStringAsync().Result;
            JT808ResultDto<int> jt808Result5 = JsonConvert.DeserializeObject<JT808ResultDto<int>>(content5);
            Assert.Equal(200, jt808Result5.Code);
            Assert.Equal(length, jt808Result5.Data);
        }

        [Fact]
        public void GetByChannelIdTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRealAll").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<IEnumerable<JT808SessionInfoDto>> jt808Result1 = JsonConvert.DeserializeObject<JT808ResultDto<IEnumerable<JT808SessionInfoDto>>>(content);
            var sessionInfo1 = jt808Result1.Data.FirstOrDefault();

            var result2 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/GetByChannelId",new StringContent(sessionInfo1.ChannelId)).Result;
            string content2 = result2.Content.ReadAsStringAsync().Result;

            JT808ResultDto<JT808SessionInfoDto> jt808Result2 = JsonConvert.DeserializeObject<JT808ResultDto<JT808SessionInfoDto>>(content2);
            Assert.Equal(200, jt808Result2.Code);
            Assert.Equal(sessionInfo1.ChannelId, jt808Result2.Data.ChannelId);
        }

        [Fact]
        public void GetByTerminalPhoneNoTest()
        {
            var result = httpClient.GetAsync($"{Url}/{sessionRoutePrefix}/GetRelevanceAll").Result;
            string content = result.Content.ReadAsStringAsync().Result;
            JT808ResultDto<IEnumerable<JT808SessionInfoDto>> jt808Result1 = JsonConvert.DeserializeObject<JT808ResultDto<IEnumerable<JT808SessionInfoDto>>>(content);
            var sessionInfo1 = jt808Result1.Data.FirstOrDefault();

            var result2 = httpClient.PostAsync($"{Url}/{sessionRoutePrefix}/GetByTerminalPhoneNo", new StringContent(sessionInfo1.TerminalPhoneNo)).Result;
            string content2 = result2.Content.ReadAsStringAsync().Result;

            JT808ResultDto<JT808SessionInfoDto> jt808Result2 = JsonConvert.DeserializeObject<JT808ResultDto<JT808SessionInfoDto>>(content2);
            Assert.Equal(200, jt808Result2.Code);
            Assert.Equal(sessionInfo1.TerminalPhoneNo, jt808Result2.Data.TerminalPhoneNo);
        }

        public override void Dispose()
        {
            base.Dispose();
            SimpleTcpClient.Down();
        }
    }
}
