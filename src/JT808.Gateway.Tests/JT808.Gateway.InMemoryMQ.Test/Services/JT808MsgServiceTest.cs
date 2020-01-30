using JT808.Gateway.InMemoryMQ.Services;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JT808.Gateway.InMemoryMQ.Test.Services
{
    public class JT808MsgServiceTest
    {
        [Fact]
        public void Test1()
        {
            JT808MsgService jT808MsgService = new JT808MsgService();
            jT808MsgService.WriteAsync("132", new byte[] { 1, 2, 3 }).GetAwaiter().GetResult();
            var result = jT808MsgService.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
            Assert.Equal("132", result.TerminalNo);
            Assert.Equal(new byte[] { 1, 2, 3 }, result.Data);
        }
    }
}
