using JT808.DotNetty.Abstractions.Dtos;

namespace JT808.DotNetty.Core.Interfaces
{
    /// <summary>
    /// JT808基于udp的统一下发命令服务
    /// </summary>
    internal interface IJT808UnificationUdpSendService
    {
        JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data);
    }
}
