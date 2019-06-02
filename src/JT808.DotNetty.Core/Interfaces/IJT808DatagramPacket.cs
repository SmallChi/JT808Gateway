using DotNetty.Transport.Channels.Sockets;
using System.Net;

namespace JT808.DotNetty.Core.Interfaces
{
    /// <summary>
    /// 基于udp的创建发送包
    /// </summary>
    interface IJT808DatagramPacket
    {
        DatagramPacket Create(byte[] message, EndPoint recipient);
    }
}
