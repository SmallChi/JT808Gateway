using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Codecs.Http;
using DotNetty.Common.Utilities;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.WebApi.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace JT808.DotNetty.WebApi.Test.Authorization
{
    public class JT808AuthorizationDefaultTest
    {
        [Fact]
        public void AuthorizationQuertStringTest()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.Configure<JT808Configuration>((options)=> { });
            var options=serviceDescriptors.BuildServiceProvider().GetRequiredService<IOptionsMonitor<JT808Configuration>>();
            JT808AuthorizationDefault jT808AuthorizationDefault = new JT808AuthorizationDefault(options);
            var m = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Get, "/demo?token=123456");
            Assert.True(jT808AuthorizationDefault.Authorization(m, out var principal));
        }

        [Fact]
        public void AuthorizationQuertStringFailTest()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.Configure<JT808Configuration>((options) => { });
            var options = serviceDescriptors.BuildServiceProvider().GetRequiredService<IOptionsMonitor<JT808Configuration>>();
            JT808AuthorizationDefault jT808AuthorizationDefault = new JT808AuthorizationDefault(options);
            var m = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Get, "/demo?token=12345");
            Assert.False(jT808AuthorizationDefault.Authorization(m, out var principal));
        }

        [Fact]
        public void AuthorizationHeaderTest()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.Configure<JT808Configuration>((options) => { });
            var options = serviceDescriptors.BuildServiceProvider().GetRequiredService<IOptionsMonitor<JT808Configuration>>();
            JT808AuthorizationDefault jT808AuthorizationDefault = new JT808AuthorizationDefault(options);
            var m = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Get, "/");
            m.Headers.Add((AsciiString)"token", "123456");
            Assert.True(jT808AuthorizationDefault.Authorization(m, out var principal));
        }

        [Fact]
        public void AuthorizationHeaderFailTest()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.Configure<JT808Configuration>((options) => { });
            var options = serviceDescriptors.BuildServiceProvider().GetRequiredService<IOptionsMonitor<JT808Configuration>>();
            JT808AuthorizationDefault jT808AuthorizationDefault = new JT808AuthorizationDefault(options);
            var m = new DefaultFullHttpRequest(HttpVersion.Http11, HttpMethod.Get, "/");
            m.Headers.Add((AsciiString)"token", "12345");
            Assert.False(jT808AuthorizationDefault.Authorization(m, out var principal));
        }
    }
}
