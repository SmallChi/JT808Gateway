using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.WebApiClientTool;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace JT808.DotNetty.WebApi.Test
{
    public class JT808HttpClientTest
    {
        public static HttpClient CreateHttpClient(string uri,string requestjson,string responseJson)
        {
            string baseUrl = "http://localhost";
            var mockHttp = new MockHttpMessageHandler();
            var request = mockHttp.When($"{baseUrl}{uri}")
                                    .Respond("application/json", responseJson);
            if (!string.IsNullOrEmpty(requestjson))
            {
                request.WithContent(requestjson);
            }
            var client = mockHttp.ToHttpClient();

            client.BaseAddress = new Uri(baseUrl);
            return client;
        }

        [Fact]
        public void GetTcpSessionAllTest()
        {
            JT808ResultDto<List<JT808TcpSessionInfoDto>> jT808ResultDto = new JT808ResultDto<List<JT808TcpSessionInfoDto>>();
            jT808ResultDto.Data = new List<JT808TcpSessionInfoDto>();
            jT808ResultDto.Code = 200;
            jT808ResultDto.Data.Add(new JT808TcpSessionInfoDto { 
                 LastActiveTime=DateTime.Parse("2019-10-29 23:23:23"),
                 StartTime=DateTime.Parse("2019-10-29 23:23:23"),
                 RemoteAddressIP="127.0.0.1:555",
                 TerminalPhoneNo="123456789"
            });
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll,"", JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.GetTcpSessionAll();
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data[0].TerminalPhoneNo, result.Data[0].TerminalPhoneNo);
            Assert.Equal(jT808ResultDto.Data[0].StartTime, result.Data[0].StartTime);
            Assert.Equal(jT808ResultDto.Data[0].LastActiveTime, result.Data[0].LastActiveTime);
            Assert.Equal(jT808ResultDto.Data[0].RemoteAddressIP, result.Data[0].RemoteAddressIP);
        }

        [Fact]
        public void GetTcpSessionAllLargeTest()
        {
            JT808ResultDto<List<JT808TcpSessionInfoDto>> jT808ResultDto = new JT808ResultDto<List<JT808TcpSessionInfoDto>>();
            jT808ResultDto.Data = new List<JT808TcpSessionInfoDto>();
            jT808ResultDto.Code = 200;
            for(var i = 0; i < 50000; i++)
            {
                jT808ResultDto.Data.Add(new JT808TcpSessionInfoDto
                {
                    LastActiveTime = DateTime.Parse("2019-10-29 23:23:23"),
                    StartTime = DateTime.Parse("2019-10-29 23:23:23"),
                    RemoteAddressIP = "127.0.0.1:555",
                    TerminalPhoneNo = (i+1).ToString()
                });
            }
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll, "", JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.GetTcpSessionAll();
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(50000, result.Data.Count);
        }

        [Fact]
        public void RemoveSessionByTerminalPhoneNoTest()
        {
            JT808ResultDto<bool> jT808ResultDto = new JT808ResultDto<bool>();
            jT808ResultDto.Data = true;
            jT808ResultDto.Code = 200;
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo, "123456789",  JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.RemoveByTerminalPhoneNo("123456789");
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data, result.Data);
        }

        [Fact]
        public void UnificationSendTest()
        {
            JT808ResultDto<bool> jT808ResultDto = new JT808ResultDto<bool>();
            jT808ResultDto.Data = true;
            jT808ResultDto.Code = 200;
            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = new JT808UnificationSendRequestDto
            {
                TerminalPhoneNo = "123456789",
                Data = new byte[] { 1, 2, 3, 4 }
            };
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.UnificationSend, JsonSerializer.Serialize(jT808UnificationSendRequestDto),  JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.UnificationSend(jT808UnificationSendRequestDto);
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data, result.Data);
        }

        [Fact]
        public void GetTcpAtomicCounterTest()
        {
            JT808ResultDto<JT808AtomicCounterDto> jT808ResultDto = new JT808ResultDto<JT808AtomicCounterDto>();
            jT808ResultDto.Data = new JT808AtomicCounterDto {
                 MsgFailCount=9,
                 MsgSuccessCount=10
            };
            jT808ResultDto.Code = 200;
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.GetTcpAtomicCounter, "",JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.GetTcpAtomicCounter();
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data.MsgFailCount, result.Data.MsgFailCount);
            Assert.Equal(jT808ResultDto.Data.MsgSuccessCount, result.Data.MsgSuccessCount);
        }

        [Fact]
        public void GetUdpAtomicCounterTest()
        {
            JT808ResultDto<JT808AtomicCounterDto> jT808ResultDto = new JT808ResultDto<JT808AtomicCounterDto>();
            jT808ResultDto.Data = new JT808AtomicCounterDto
            {
                MsgFailCount = 19,
                MsgSuccessCount = 110
            };
            jT808ResultDto.Code = 200;
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.GetUdpAtomicCounter, "", JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.GetUdpAtomicCounter();
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data.MsgFailCount, result.Data.MsgFailCount);
            Assert.Equal(jT808ResultDto.Data.MsgSuccessCount, result.Data.MsgSuccessCount);
        }

        [Fact]
        public void GetUdpSessionAllTest()
        {
            JT808ResultDto<List<JT808UdpSessionInfoDto>> jT808ResultDto = new JT808ResultDto<List<JT808UdpSessionInfoDto>>();
            jT808ResultDto.Data = new List<JT808UdpSessionInfoDto>();
            jT808ResultDto.Data.Add(new JT808UdpSessionInfoDto
            {
                LastActiveTime = DateTime.Parse("2019-10-29 21:21:21"),
                StartTime = DateTime.Parse("2019-10-29 21:21:21"),
                RemoteAddressIP = "127.0.0.1:666",
                TerminalPhoneNo = "123456789"
            });
            jT808ResultDto.Code = 200;
            JT808HttpClient jT808HttpClient = new JT808HttpClient(CreateHttpClient(JT808NettyConstants.JT808WebApiRouteTable.SessionUdpGetAll, "", JsonSerializer.Serialize(jT808ResultDto)));
            var result = jT808HttpClient.GetUdpSessionAll();
            Assert.Equal(jT808ResultDto.Code, result.Code);
            Assert.Equal(jT808ResultDto.Data[0].TerminalPhoneNo, result.Data[0].TerminalPhoneNo);
            Assert.Equal(jT808ResultDto.Data[0].StartTime, result.Data[0].StartTime);
            Assert.Equal(jT808ResultDto.Data[0].LastActiveTime, result.Data[0].LastActiveTime);
            Assert.Equal(jT808ResultDto.Data[0].RemoteAddressIP, result.Data[0].RemoteAddressIP);
        }
    }
}
