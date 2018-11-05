using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dtos
{
    public class JT808SessionInfoDto
    {
        /// <summary>
        /// 通道Id
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 最后上线时间
        /// </summary>
        public DateTime LastActiveTime { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }
    }
}
