using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JT808.DotNetty.Core.Test
{
    [TestClass]
    public class JT808TransmitAddressFilterServiceTest
    {
        private JT808TransmitAddressFilterService jT808TransmitAddressFilterService;

        public JT808TransmitAddressFilterServiceTest()
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
                services.AddSingleton<JT808TransmitAddressFilterService>();
            });
            var serviceProvider = serverHostBuilder.Build().Services;
            jT808TransmitAddressFilterService = serviceProvider.GetService<JT808TransmitAddressFilterService>();
            jT808TransmitAddressFilterService.Add("127.0.0.1");
        }

        [TestMethod]
        public void Test1()
        {
            Assert.IsTrue(jT808TransmitAddressFilterService.ContainsKey(new JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 12348
            }.EndPoint));
        }

        [TestMethod]
        public void Test2()
        {
            var result = jT808TransmitAddressFilterService.GetAll();
        }

        [TestMethod]
        public void Test3()
        {
            var result1= jT808TransmitAddressFilterService.Add("127.0.0.1");
            Assert.AreEqual(JT808ResultCode.Ok, result1.Code);
            Assert.IsTrue(result1.Data);
            var result2 = jT808TransmitAddressFilterService.Remove("127.0.0.1");
            Assert.AreEqual(JT808ResultCode.Ok, result2.Code);
            Assert.IsTrue(result2.Data);
        }

        [TestMethod]
        public void Test4()
        {
            var result2 = jT808TransmitAddressFilterService.Remove("127.0.0.1");
            Assert.AreEqual(JT808ResultCode.Ok, result2.Code);
            Assert.IsFalse(result2.Data);
            Assert.AreEqual("不能删除服务器配置的地址", result2.Message);
        }
    }
}
