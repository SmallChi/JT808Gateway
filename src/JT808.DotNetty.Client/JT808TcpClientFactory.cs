using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JT808.DotNetty.Client
{
    public interface IJT808TcpClientFactory : IDisposable
    {
        JT808TcpClient Create(JT808DeviceConfig deviceConfig);

        List<JT808TcpClient> GetAll();
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

        public JT808TcpClient Create(JT808DeviceConfig deviceConfig)
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

        public List<JT808TcpClient> GetAll()
        {
            return dict.Values.ToList();
        }
    }
}
