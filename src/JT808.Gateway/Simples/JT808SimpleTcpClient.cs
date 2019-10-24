using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Simples
{
    internal class JT808SimpleTcpClient
    {
        private TcpClient tcpClient;

        public JT808SimpleTcpClient(IPEndPoint remoteAddress)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(remoteAddress);
            Task.Run(()=> {
                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[100];
                        tcpClient.GetStream().Read(buffer, 0, 100);
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " " + string.Join(" ", buffer));
                        
                    }
                    catch 
                    {

                       
                    }
                    Thread.Sleep(1000);
                }
            });
        }
        
       

        public void WriteAsync(byte[] data)
        {
            tcpClient.GetStream().WriteAsync(data, 0, data.Length);
        }

        public void Down()
        {
            tcpClient.Close();
        }
    }
}
