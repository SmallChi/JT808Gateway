using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.WebApi.Authorization;
using JT808.DotNetty.WebApi.Handlers;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using HttpVersion = DotNetty.Codecs.Http.HttpVersion;

namespace JT808.DotNetty.WebApi.Test.Handlers
{
    public class JT808WebAPIServerHandlerTest
    {
        [Fact]
        public void SessionTcpGetAllTest()
        {
            var ch = WebExt.CreateEmbeddedChannel(serviceProvider => {
                var sessionMgr = serviceProvider.GetRequiredService<JT808SessionManager>();
                sessionMgr.TryAdd("12345678",new EmbeddedChannel());
            });
            ch.WriteInbound(WebExt.CreateGetRequest(JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll));
            var result = ch.As<List<JT808TcpSessionInfoDto>>();
            Assert.Single(result.Data);
            Assert.Equal("12345678", result.Data[0].TerminalPhoneNo);
            Assert.Equal(JT808ResultCode.Ok, result.Code);
        }      

        [Fact]
        public void GetUdpSessionAllTest()
        {
            var ch = WebExt.CreateEmbeddedChannel(serviceProvider => {
                var sessionMgr=serviceProvider.GetRequiredService<JT808SessionManager>();
                sessionMgr.TryAdd(new EmbeddedChannel(),new IPEndPoint(IPAddress.Parse("127.0.0.1"), 888),"123456789");
            });
            ch.WriteInbound(WebExt.CreateGetRequest(JT808NettyConstants.JT808WebApiRouteTable.SessionUdpGetAll));
            var result = ch.As<List<JT808UdpSessionInfoDto>>();
            Assert.Single(result.Data);
            Assert.Equal("123456789",result.Data[0].TerminalPhoneNo);
            Assert.Equal(JT808ResultCode.Ok, result.Code);
        }

        [Fact]
        public void RemoveSessionByTerminalPhoneNoTest()
        {
            var ch = WebExt.CreateEmbeddedChannel();
            ch.WriteInbound(WebExt.CreatePostRequest(JT808NettyConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo,Encoding.UTF8.GetBytes("12345678")));
            var result = ch.As<bool>();
            Assert.False(result.Data);
            Assert.Equal(JT808ResultCode.Empty, result.Code);
            Assert.Equal("Session Empty", result.Message);
        }

        [Fact]
        public void UnificationSendTest()
        {
            var ch = WebExt.CreateEmbeddedChannel();
            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = new JT808UnificationSendRequestDto
            {
                TerminalPhoneNo = "123456789",
                Data = new byte[] { 1, 2, 3, 4 }
            };
            byte[] content = JsonSerializer.SerializeToUtf8Bytes(jT808UnificationSendRequestDto);
            ch.WriteInbound(WebExt.CreatePostRequest(JT808NettyConstants.JT808WebApiRouteTable.UnificationSend, content));
            var result = ch.As<bool>();
            Assert.False(result.Data);
            Assert.Equal(JT808ResultCode.Ok, result.Code);
            Assert.Equal("offline", result.Message);
        }        

        [Fact]
        public void GetTcpAtomicCounterTest()
        {
            var ch = WebExt.CreateEmbeddedChannel(serviceProvider=> 
            {
                var counterFactory = serviceProvider.GetRequiredService<JT808AtomicCounterServiceFactory>();
                var counter = counterFactory.Create(JT808TransportProtocolType.tcp);
                counter.MsgSuccessIncrement();
                counter.MsgSuccessIncrement();
                counter.MsgFailIncrement();
            });

            ch.WriteInbound(WebExt.CreateGetRequest(JT808NettyConstants.JT808WebApiRouteTable.GetTcpAtomicCounter));
            var result = ch.As<JT808AtomicCounterDto>();
            Assert.Equal(2,result.Data.MsgSuccessCount);
            Assert.Equal(1,result.Data.MsgFailCount);
            Assert.Equal(JT808ResultCode.Ok, result.Code);
        }

        [Fact]
        public void GetUdpAtomicCounterTest()
        {
            var ch = WebExt.CreateEmbeddedChannel(serviceProvider =>
            {
                var counterFactory = serviceProvider.GetRequiredService<JT808AtomicCounterServiceFactory>();
                var counter = counterFactory.Create(JT808TransportProtocolType.udp);
                counter.MsgSuccessIncrement();
                counter.MsgSuccessIncrement();
                counter.MsgSuccessIncrement();
                counter.MsgFailIncrement();
            });

            ch.WriteInbound(WebExt.CreateGetRequest(JT808NettyConstants.JT808WebApiRouteTable.GetUdpAtomicCounter));
            var result = ch.As<JT808AtomicCounterDto>();
            Assert.Equal(3, result.Data.MsgSuccessCount);
            Assert.Equal(1, result.Data.MsgFailCount);
            Assert.Equal(JT808ResultCode.Ok, result.Code);
        }

        [Fact]
        public void UriTest1()
        {
            string uri = JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll +"? token=123456";
            var uriSpan = uri.AsSpan();
            var index = uriSpan.IndexOf('?');
            var result = uriSpan.Slice(0, index).ToString();
            Assert.Equal(JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll, result);
        }

        [Fact]
        public void UriTest2()
        {
            var index = JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll.IndexOf('?');
            Assert.Equal(-1, index);
        }
    }

    public static class WebExt
    {
        public static EmbeddedChannel CreateEmbeddedChannel()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddJT808Configure()
                                .AddJT808NettyCore(options => { })
                                .Builder();
            serviceDescriptors.AddJT808WebApiNettyHostTest();
            var handler = serviceDescriptors.BuildServiceProvider().GetRequiredService<JT808WebAPIServerHandler>();
            var ch = new EmbeddedChannel(
                    new HttpRequestDecoder(4096, 8192, 8192, false),
                    new HttpObjectAggregator(int.MaxValue),
                    handler);
            return ch;
        }

        public static EmbeddedChannel CreateEmbeddedChannel(Action<IServiceProvider> action)
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddJT808Configure()
                                .AddJT808NettyCore(options => { })
                                .Builder();
            serviceDescriptors.AddSingleton<JT808MsgIdHttpHandlerBase, JT808MsgIdDefaultWebApiHandler>();
            serviceDescriptors.AddSingleton<IJT808WebApiAuthorization, JT808AuthorizationDefault>();
            serviceDescriptors.AddScoped<JT808WebAPIServerHandler>();
            var serviceProvider = serviceDescriptors.BuildServiceProvider();
            var handler = serviceProvider.GetRequiredService<JT808WebAPIServerHandler>();
            var ch = new EmbeddedChannel(
                    new HttpRequestDecoder(4096, 8192, 8192, false),
                    new HttpObjectAggregator(int.MaxValue),
                    handler);
            action(serviceProvider);
            return ch;
        }

        public static JT808ResultDto<T> As<T>(this EmbeddedChannel embeddedChannel)
        {
            if(embeddedChannel == null) return default;
            DefaultFullHttpResponse response = embeddedChannel.ReadOutbound<DefaultFullHttpResponse>();
            if (response.Headers.TryGetInt(HttpHeaderNames.ContentLength, out int length))
            {
                byte[] tmp = new byte[length];
                response.Content.ReadBytes(tmp);
                response.Release();
                return JsonSerializer.Deserialize<JT808ResultDto<T>>(tmp);
            }
            response.Release();
            return default;
        }
        public static DefaultFullHttpRequest CreateGetRequest(string uri)
        {
            var request = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Get, uri);
            request.Headers.Add((AsciiString)"token", "123456");
            return request;
        }
        public static DefaultFullHttpRequest CreatePostRequest(string uri,byte[]content)
        {
            var request = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Post, uri);
            request.Headers.Add((AsciiString)"token", "123456");
            request.Content.WriteBytes(content);
            return request;
        }
    }
}
