using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core
{
    internal class JT808SimpleUdpClient
    {
        private UdpClient udpClient;

        public JT808SimpleUdpClient(IPEndPoint remoteAddress)
        {
            udpClient = new UdpClient();
            udpClient.Connect(remoteAddress);
            Task.Run(() => {
                while (true)
                {
                    string tmp = string.Join(" ", udpClient.Receive(ref remoteAddress));
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " " + tmp);
                    Thread.Sleep(1000);
                }
            });
        }

        public void WriteAsync(byte[] data)
        {
            udpClient.SendAsync(data, data.Length);
        }

        public void Down()
        {
            udpClient.Close();
        }
    }
}
