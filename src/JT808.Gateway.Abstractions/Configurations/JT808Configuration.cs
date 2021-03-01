using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions.Configurations
{
    /// <summary>
    /// JT808网关配置
    /// </summary>
    public class JT808Configuration
    {
        /// <summary>
        /// tcp端口
        /// </summary>
        public int TcpPort { get; set; } = 808;
        /// <summary>
        /// udp端口
        /// </summary>
        public int UdpPort { get; set; } = 808;
        /// <summary>
        /// http webapi端口
        /// </summary>
        public int WebApiPort { get; set; } = 828;
        /// <summary>
        /// WebApi 默认token 123456 
        /// </summary>
        public string WebApiToken { get; set; } = "123456";
        /// <summary>
        /// tcp连接能够成功连接上的数量
        /// </summary>
        public int SoBacklog { get; set; } = 10000;
        /// <summary>
        /// 默认4k
        /// </summary>
        public int MiniNumBufferSize { get; set; } = 4096;
        /// <summary>
        /// Tcp读超时 
        /// 默认10分钟检查一次
        /// </summary>
        public int TcpReaderIdleTimeSeconds { get; set; } = 60 * 10;
        /// <summary>
        /// Tcp 60s检查一次
        /// </summary>
        public int TcpReceiveTimeoutCheckTimeSeconds { get; set; } = 60;
        /// <summary>
        /// Udp读超时
        /// </summary>
        public int UdpReaderIdleTimeSeconds { get; set; } = 60;
        /// <summary>
        /// Udp 60s检查一次
        /// </summary>
        public int UdpReceiveTimeoutCheckTimeSeconds { get; set; } = 60;
        /// <summary>
        /// 网关忽略消息应答
        /// </summary>
        public HashSet<uint> IgnoreMsgIdReply { get; set; } = new HashSet<uint>();
    }
}
