using JT808.DotNetty.Configurations;
using JT808.DotNetty.Dtos;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808RemoteAddressTransmitConfigurationServiceTest
    {
        private JT808RemoteAddressTransmitConfigurationService jT808RemoteAddressTransmitConfigurationService;

        public JT808RemoteAddressTransmitConfigurationServiceTest()
        {
            var serverHostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.Configure<JT808Configuration>(hostContext.Configuration.GetSection("JT808Configuration"));
                services.AddSingleton<JT808RemoteAddressTransmitConfigurationService>();
            });
            var serviceProvider = serverHostBuilder.Build().Services;
            jT808RemoteAddressTransmitConfigurationService = serviceProvider.GetService<JT808RemoteAddressTransmitConfigurationService>();
            jT808RemoteAddressTransmitConfigurationService.Add(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12345
            });
            jT808RemoteAddressTransmitConfigurationService.Add(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12346
            });
            jT808RemoteAddressTransmitConfigurationService.Add(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12347
            });
            jT808RemoteAddressTransmitConfigurationService.Add(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12348
            });
        }

        [Fact]
        public void Test1()
        {
            Assert.True(jT808RemoteAddressTransmitConfigurationService.ContainsKey(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12348
            }.EndPoint));
        }

        [Fact]
        public void Test2()
        {
            var result = jT808RemoteAddressTransmitConfigurationService.GetAll();
        }

        [Fact]
        public void Test3()
        {
            var ip1 = new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12349
            };
            var result1= jT808RemoteAddressTransmitConfigurationService.Add(ip1);
            Assert.Equal(JT808ResultCode.Ok, result1.Code);
            Assert.True(result1.Data);
            var result2 = jT808RemoteAddressTransmitConfigurationService.Remove(ip1);
            Assert.Equal(JT808ResultCode.Ok, result2.Code);
            Assert.True(result2.Data);
        }

        [Fact]
        public void Test4()
        {
            var configIp = new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 6561
            };
            var result2 = jT808RemoteAddressTransmitConfigurationService.Remove(configIp);
            Assert.Equal(JT808ResultCode.Ok, result2.Code);
            Assert.False(result2.Data);
            Assert.Equal("不能删除服务器配置的地址", result2.Message);  
        }
    }
}
