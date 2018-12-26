using System;

namespace JT808.DotNetty.Abstractions.Dtos
{
    public class JT808TcpSessionInfoDto
    {
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
        /// <summary>
        /// 远程ip地址
        /// </summary>
        public string RemoteAddressIP { get; set; }
    }
}
