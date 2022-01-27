using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Abstractions
{
    /// <summary>
    /// JT808会话扩展
    /// </summary>
    public static class JT808SessionExtensions
    {
        /// <summary>
        /// 下发消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        public static async void SendAsync(this IJT808Session session,byte[] data)
        {
            try
            {
                if (data == null) return;
                if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                {
                    if (session.Client.Connected)
                        await session.Client.SendAsync(data, SocketFlags.None);
                }
                else
                {
                    await session.Client.SendToAsync(data, SocketFlags.None, session.RemoteEndPoint);
                }
            }
            catch (AggregateException ex)
            {

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 下发消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        public static void Send(this IJT808Session session, byte[] data)
        {
            try
            {
                if (data == null) return;
                if (session.TransportProtocolType == JT808TransportProtocolType.tcp)
                {
                    if (session.Client.Connected)
                        session.Client.Send(data, SocketFlags.None);
                }
                else
                {
                    session.Client.SendTo(data, SocketFlags.None, session.RemoteEndPoint);
                }
            }
            catch (AggregateException ex)
            {

            }
            catch (Exception)
            {

            }
        }
    }
}
