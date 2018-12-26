using DotNetty.Transport.Channels;
using System;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808TcpSession
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
    }
}
