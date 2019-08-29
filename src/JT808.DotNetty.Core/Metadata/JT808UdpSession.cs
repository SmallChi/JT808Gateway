using DotNetty.Transport.Channels;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Interfaces;
using System;
using System.Net;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808UdpSession: IJT808Session
    {
        public JT808UdpSession(IChannel channel,
            EndPoint sender,
            string terminalPhoneNo)
        {
            Channel = channel;
            TerminalPhoneNo = terminalPhoneNo;
            StartTime = DateTime.Now;
            LastActiveTime = DateTime.Now;
            Sender = sender;
        }

        public EndPoint Sender { get; set; }

        public JT808UdpSession() { }

        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }

        public IChannel Channel { get; set; }

        public DateTime LastActiveTime { get; set; }

        public DateTime StartTime { get; set; }
        public JT808TransportProtocolType TransportProtocolType { get; set; } = JT808TransportProtocolType.udp;
    }
}
