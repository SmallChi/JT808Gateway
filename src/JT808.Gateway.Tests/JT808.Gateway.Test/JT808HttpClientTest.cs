using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JT808.Gateway.WebApiClientTool;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JT808.Gateway.Test
{
    public class JT808HttpClientTest
    {
        [Fact(DisplayName="客户端DI测试配置文件1")]
        public void Test()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddJT808WebApiClientTool(new Uri("http://localhost/"), "123456");
        }

        [Fact(DisplayName = "客户端DI测试配置文件2")]
        public void Test1()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
            IServiceCollection serviceDescriptors = new ServiceCollection();
            var configuration = builder.Build();
            var token = configuration.GetSection("JT808WebApiClientToolConfig:Token").Get<string>();
            var uri = configuration.GetSection("JT808WebApiClientToolConfig:Uri").Get<string>();
            Assert.Equal("123456", token);
            Assert.Equal("http://localhost/", uri);
            serviceDescriptors.AddJT808WebApiClientTool(builder.Build());
        }        
        [Fact(DisplayName = "使用postman测试")]
        public void Test2()
        {
            //头部添加或者url参数带有token=123456
            //TCP
            //http://127.0.0.1:828/jt808api/Tcp/Session/GetAll
            //http://127.0.0.1:828/jt808api/Tcp/Session/QueryTcpSessionByTerminalPhoneNo
            //123456789
            //http://127.0.0.1:828/jt808api/Tcp/Session/RemoveByTerminalPhoneNo
            //123456789
            ///http://127.0.0.1:828/jt808api/UnificationSend
            //{"TerminalPhoneNo":"123456789","HexData":"7e 01 02 7e"}
            //{"TerminalPhoneNo":"123456789","HexData":"7e01027e"}
            //UDP
            //http://127.0.0.1:828/jt808api/Udp/Session/GetAll
            //http://127.0.0.1:828/jt808api/Udp/Session/RemoveUdpByTerminalPhoneNo
            //123456789
            //http://127.0.0.1:828/jt808api/Udp/Session/QueryUdpSessionByTerminalPhoneNo
        }
    }
}
