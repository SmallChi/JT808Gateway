using JT808.DotNetty.Abstractions.Enums;
using System.Threading.Tasks;

namespace JT808.DotNetty.Abstractions
{
    /// <summary>
    /// 上行数据包处理接口
    /// </summary>
    public interface IJT808UplinkPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="transportProtocolType">传输协议类型</param>
        /// <returns></returns>
        Task ProcessorAsync(byte[] data, JT808TransportProtocolType  transportProtocolType);
    }
}
