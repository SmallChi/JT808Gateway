using DotNetty.Transport.Channels;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Interfaces;
using System;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808TcpSession: IJT808Session
    {
        public JT808TcpSession(IChannel channel, string terminalPhoneNo)
        {
            Channel = channel;
            TerminalPhoneNo = terminalPhoneNo;
            StartTime = DateTime.Now;
            LastActiveTime = DateTime.Now;
        }

        public JT808TcpSession() { }

        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }

        public IChannel Channel { get; set; }

        public DateTime LastActiveTime { get; set; }

        public DateTime StartTime { get; set; }
        public JT808TransportProtocolType TransportProtocolType { get; set; } = JT808TransportProtocolType.tcp;
    }
}
