using JT808.Gateway.Dtos;

namespace JT808.Gateway.Interfaces
{
    /// <summary>
    /// JT808统一下发命令服务
    /// </summary>
    public interface IJT808UnificationSendService
    {
        JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data);
    }
}
