using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace JT808.Gateway.InMemoryMQ.Test.Services
{
    public class JT808SessionServiceTest
    {
        [Fact]
        public void Test1()
        {
            JT808SessionService jT808SessionService = new JT808SessionService();
            jT808SessionService.WriteAsync(JT808GatewayConstants.SessionOnline, "123456").GetAwaiter().GetResult();
            jT808SessionService.WriteAsync(JT808GatewayConstants.SessionOffline, "123457").GetAwaiter().GetResult();
            jT808SessionService.WriteAsync(JT808GatewayConstants.SessionOnline, "123456,123457").GetAwaiter().GetResult();
            var result1 = jT808SessionService.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
            var result2 = jT808SessionService.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
            var result3 = jT808SessionService.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
            Assert.Equal(JT808GatewayConstants.SessionOnline, result1.Notice);
            Assert.Equal("123456", result1.TerminalNo);
            Assert.Equal(JT808GatewayConstants.SessionOffline, result2.Notice);
            Assert.Equal("123457", result2.TerminalNo);
            //转发
            Assert.Equal(JT808GatewayConstants.SessionOnline, result3.Notice);
            Assert.Equal("123456,123457", result3.TerminalNo);
        }
    }
}
