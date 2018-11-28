using JT808.DotNetty.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Interfaces
{
    /// <summary>
    /// JT808会话服务
    /// </summary>
    internal interface IJT808SessionService
    {
        /// <summary>
        /// 获取会话集合
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<List<JT808SessionInfoDto>> GetAll();
        /// <summary>
        /// 通过通道Id移除对应会话
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        JT808ResultDto<bool> RemoveByChannelId(string channelId);
        /// <summary>
        /// 通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        JT808ResultDto<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo);
    }
}
