using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Client
{
    public interface IJT808TcpClientFactory : IDisposable
    {
        JT808TcpClient Create(DeviceConfig deviceConfig);
    }

    public class JT808TcpClientFactory: IJT808TcpClientFactory
    {
        private readonly ConcurrentDictionary<string, JT808TcpClient> dict;

        private readonly IServiceProvider serviceProvider;

        public JT808TcpClientFactory(IServiceProvider serviceProvider)
        {
            dict = new ConcurrentDictionary<string, JT808TcpClient>(StringComparer.OrdinalIgnoreCase);
            this.serviceProvider = serviceProvider;
        }

        public JT808TcpClient Create(DeviceConfig deviceConfig)
        {
            if(dict.TryGetValue(deviceConfig.TerminalPhoneNo,out var client))
            {
                return client;
            }
            else
            {
                JT808TcpClient jT808TcpClient = new JT808TcpClient(deviceConfig, serviceProvider);
                dict.TryAdd(deviceConfig.TerminalPhoneNo, jT808TcpClient);
                return jT808TcpClient;
            }
        }

        public void Dispose()
        {
            foreach(var client in dict)
            {
                try
                {
                    client.Value.Dispose();
                }
                catch 
                {
                }
            }
        }
    }
}
