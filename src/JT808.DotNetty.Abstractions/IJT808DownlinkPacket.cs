using JT808.DotNetty.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Abstractions
{
    /// <summary>
    /// 下行数据包处理接口
    /// </summary>
    public interface IJT808DownlinkPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="protocolType">协议类型:tcp/udp</param>
        /// <returns></returns>
        Task ProcessorAsync(byte[] data, JT808TransportProtocolType transportProtocolType);
    }
}
