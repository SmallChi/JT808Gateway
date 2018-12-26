using JT808.DotNetty.Abstractions.Dtos;

namespace JT808.DotNetty.Core.Interfaces
{
    /// <summary>
    /// JT808基于tcp的统一下发命令服务
    /// </summary>
    public interface IJT808UnificationTcpSendService
    {
        JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data);
    }
}
