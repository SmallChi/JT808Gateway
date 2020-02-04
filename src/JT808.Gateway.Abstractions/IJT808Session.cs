using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808Session
    {
        /// <summary>
        /// 终端手机号
        /// </summary>
        string TerminalPhoneNo { get; set; }
        string SessionID { get; }
        Socket Client { get; set; }
        DateTime StartTime { get; set; }
        DateTime ActiveTime { get; set; }
        JT808TransportProtocolType TransportProtocolType { get;}
        CancellationTokenSource ReceiveTimeout { get; set; }
        EndPoint RemoteEndPoint { get; set; }
        void Close();
    }
}
