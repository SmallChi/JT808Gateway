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
        /// 获取真实连接数
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<int> GetRealLinkCount();
        /// <summary>
        /// 获取设备相关连的连接数
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<int> GetRelevanceLinkCount();
        /// <summary>
        /// 获取实际会话集合
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<List<JT808SessionInfoDto>> GetRealAll();
        /// <summary>
        /// 获取设备相关联会话集合
        /// </summary>
        /// <returns></returns>
        JT808ResultDto<List<JT808SessionInfoDto>> GetRelevanceAll();
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
        /// <summary>
        /// 通过通道Id获取会话信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        JT808ResultDto<JT808SessionInfoDto> GetByChannelId(string channelId);
        /// <summary>
        /// 通过设备终端号获取会话信息
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        JT808ResultDto<JT808SessionInfoDto> GetByTerminalPhoneNo(string terminalPhoneNo);
    }
}
