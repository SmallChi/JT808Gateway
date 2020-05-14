using JT808.DotNetty.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
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
        /// <summary>
        /// 通过设备终端号获取对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        JT808ResultDto<JT808TcpSessionInfoDto> QueryTcpSessionByTerminalPhoneNo(string terminalPhoneNo);
    }
}
