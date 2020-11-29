using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Abstractions
{
    public static class JT808SessionExtensions
    {
        public static async void SendAsync(this IJT808Session session,byte[] data)
        {
            if (data == null) return;
            if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
            {
                await session.Client.SendAsync(data, SocketFlags.None);
            }
            else
            {
                await session.Client.SendToAsync(data, SocketFlags.None, session.RemoteEndPoint);
            }
        }

        public static void Send(this IJT808Session session, byte[] data)
        {
            if (data == null) return;
            if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
            {
               session.Client.Send(data, SocketFlags.None);
            }
            else
            {
               session.Client.SendTo(data, SocketFlags.None, session.RemoteEndPoint);
            }
        }
    }
}
