using JT808.DotNetty.Abstractions.Dtos;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace JT808.DotNetty.WebApiClientTool
{
    public interface IJT808DotNettyWebApi : IHttpApi
    {
        #region 基于Tcp WebApi
        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("Tcp/Session/GetAll")]
        ITask<JT808ResultDto<List<JT808TcpSessionInfoDto>>> GetTcpSessionAll();
        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        [HttpPost("Tcp/Session/RemoveByTerminalPhoneNo")]
        ITask<JT808ResultDto<bool>> RemoveTcpSessionByTerminalPhoneNo([JsonContent] string terminalPhoneNo);
        /// <summary>
        /// 统一下发信息
        /// </summary>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        [HttpPost("/UnificationSend")]
        ITask<JT808ResultDto<bool>> UnificationSend([JsonContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
        /// <summary>
        /// 获取Tcp包计数器
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet("Tcp/GetAtomicCounter")]
        ITask<JT808ResultDto<JT808AtomicCounterDto>> GetTcpAtomicCounter();

        #endregion

        #region 基于Udp WebApi
        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet("Udp/Session/GetAll")]
        ITask<JT808ResultDto<List<JT808UdpSessionInfoDto>>> GetUdpSessionAll();
        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        [HttpPost("Udp/Session/RemoveByTerminalPhoneNo")]
        ITask<JT808ResultDto<bool>> RemoveUdpSessionByTerminalPhoneNo([JsonContent] string terminalPhoneNo);
        /// <summary>
        /// 获取Udp包计数器
        /// </summary>
        /// <returns></returns>
        [HttpGet("Udp/GetAtomicCounter")]
        ITask<JT808ResultDto<JT808AtomicCounterDto>> GetUdpAtomicCounter();
        #endregion
    }
}
