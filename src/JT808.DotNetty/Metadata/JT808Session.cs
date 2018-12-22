using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Metadata
{
    public class JT808Session
    {
        public JT808Session(IChannel channel, string terminalPhoneNo)
        {
            Channel = channel;
            TerminalPhoneNo = terminalPhoneNo;
            StartTime = DateTime.Now;
            LastActiveTime = DateTime.Now;
        }

        public JT808Session() { }

        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }

        public IChannel Channel { get; set; }

        public DateTime LastActiveTime { get; set; }

        public DateTime StartTime { get; set; }
    }
}
