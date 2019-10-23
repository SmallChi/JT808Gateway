using JT808.Gateway.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Interfaces
{
    /// <summary>
    /// JT808会话服务
    /// </summary>
    public interface IJT808SessionService
    {
        /// <summary>
        /// 获取udp会话集合
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<List<JT808UdpSessionInfoDto>> GetUdpAll();
        /// <summary>
        /// 获取tcp会话集合
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<List<JT808TcpSessionInfoDto>> GetTcpAll();
        /// <summary>
        /// 通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        JT808ResultDto<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo);
    }
}
