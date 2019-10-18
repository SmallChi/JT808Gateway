using JT808.DotNetty.Abstractions;
using JT808.DotNetty.WebApiClientTool;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebApiClient;

namespace JT808.DotNetty.WebApi.Test
{
    [TestClass]
    public class JT808DotNettyWebApiTest: TestBase
    {
        IJT808DotNettyWebApi jT808DotNettyWebApi;

        public JT808DotNettyWebApiTest()
        {
            HttpApi.Register<IJT808DotNettyWebApi>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri("http://127.0.0.1:12828" + JT808NettyConstants.JT808WebApiRouteTable.RouteTablePrefix + "/");
                c.LoggerFactory = new LoggerFactory();
            });
            var api = HttpApi.Resolve<IJT808DotNettyWebApi>();
        }

        [TestMethod]
        public void GetUdpAtomicCounterTest()
        {
            var result = jT808DotNettyWebApi.GetUdpAtomicCounter().GetAwaiter().GetResult();
        }

        [TestMethod]
        public void UnificationUdpSendTest()
        {
            var result = jT808DotNettyWebApi.UnificationSend(new Abstractions.Dtos.JT808UnificationSendRequestDto {
                 TerminalPhoneNo= "123456789014",
                 Data=new byte[] {1,2,3,4}
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void RemoveUdpSessionByTerminalPhoneNoTest()
        {
            var result = jT808DotNettyWebApi.RemoveUdpSessionByTerminalPhoneNo("123456789014").GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetUdpSessionAllTest()
        {
            var result = jT808DotNettyWebApi.GetUdpSessionAll().GetAwaiter().GetResult();  
        }

        [TestMethod]
        public void GetTcpAtomicCounterTest()
        {
            var result = jT808DotNettyWebApi.GetTcpAtomicCounter().GetAwaiter().GetResult();
        }

        [TestMethod]
        public void UnificationTcpSendTest()
        {
            var result = jT808DotNettyWebApi.UnificationSend(new Abstractions.Dtos.JT808UnificationSendRequestDto
            {
                TerminalPhoneNo = "123456789002",
                Data = new byte[] { 1, 2, 3, 4 }
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void RemoveTcpSessionByTerminalPhoneNoTest()
        {
            var result = jT808DotNettyWebApi.RemoveTcpSessionByTerminalPhoneNo("123456789002").GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetTcpSessionAllTest()
        {
            var result = jT808DotNettyWebApi.GetTcpSessionAll().GetAwaiter().GetResult();

        }
    }
}
