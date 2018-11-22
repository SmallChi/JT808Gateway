using JT808.DotNetty.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Interfaces
{
    /// <summary>
    /// JT808统一下发命令
    /// </summary>
    internal interface IJT808UnificationSendService
    {
        JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data);
    }
}
