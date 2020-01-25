using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JT808.Gateway.Client
{
    public interface IJT808TcpClientFactory : IDisposable
    {
        ValueTask<JT808TcpClient> Create(JT808DeviceConfig deviceConfig, CancellationToken cancellationToken);

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

        public async ValueTask<JT808TcpClient> Create(JT808DeviceConfig deviceConfig, CancellationToken cancellationToken)
        {
            if(dict.TryGetValue(deviceConfig.TerminalPhoneNo,out var client))
            {
                return client;
            }
            else
            {
                JT808TcpClient jT808TcpClient = new JT808TcpClient(deviceConfig, serviceProvider);
                var successed= await jT808TcpClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(deviceConfig.TcpHost), deviceConfig.TcpPort));
                if (successed)
                {
                    await jT808TcpClient.StartAsync(cancellationToken);
                    dict.TryAdd(deviceConfig.TerminalPhoneNo, jT808TcpClient);
                    return jT808TcpClient;
                }
                return default;
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
