using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Client
{
    public class JT808DeviceConfig
    {
        public JT808DeviceConfig(string terminalPhoneNo, string tcpHost,int tcpPort, JT808Version version= JT808Version.JTT2013)
        {
            TerminalPhoneNo = terminalPhoneNo;
            TcpHost = tcpHost;
            TcpPort = tcpPort;
            Version = version;
        }
        public JT808Version Version { get; private set; }
        public string TerminalPhoneNo { get; private set; }
        public string TcpHost { get; private set; }
        public int TcpPort { get; private set; }
        /// <summary>
        /// 心跳时间（秒）
        /// </summary>
        public int Heartbeat { get; set; } = 30;

        public IJT808MsgSNDistributed MsgSNDistributed { get; }
    }
}
