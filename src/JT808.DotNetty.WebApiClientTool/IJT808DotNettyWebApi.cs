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
        /// 基于Tcp的统一下发信息
        /// </summary>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        [HttpPost("Tcp/UnificationSend")]
        ITask<JT808ResultDto<bool>> UnificationTcpSend([JsonContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
        /// <summary>
        /// 添加转发过滤地址
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpPost("Tcp/Transmit/Add")]
        ITask<JT808ResultDto<bool>> AddTransmitAddress([JsonContent]JT808IPAddressDto jT808IPAddressDto);
        /// <summary>
        /// 删除转发过滤地址（不能删除在网关服务器配置文件配的地址）
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpPost("Tcp/Transmit/Remove")]
        ITask<JT808ResultDto<bool>> RemoveTransmitAddress([JsonContent]JT808IPAddressDto jT808IPAddressDto);
        /// <summary>
        /// 获取转发过滤地址信息集合
        /// </summary>
        /// <returns></returns>
        [HttpGet("Tcp/Transmit/GetAll")]
        ITask<JT808ResultDto<List<string>>> GetTransmitAll();
        /// <summary>
        /// 获取Tcp包计数器
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet("Tcp/GetAtomicCounter")]
        ITask<JT808ResultDto<JT808AtomicCounterDto>> GetTcpAtomicCounter();
        /// <summary>
        /// 基于Tcp的流量获取
        /// </summary>
        /// <returns></returns>
        [HttpGet("Tcp/Traffic/Get")]
        ITask<JT808ResultDto<JT808TrafficInfoDto>> GetTcpTraffic();

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
        /// /基于Udp的统一下发信息
        /// </summary>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        [HttpPost("Udp/UnificationSend")]
        ITask<JT808ResultDto<bool>> UnificationUdpSend([JsonContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
        /// <summary>
        /// 基于Udp的流量获取
        /// </summary>
        /// <returns></returns>
        [HttpGet("Udp/Traffic/Get")]
        ITask<JT808ResultDto<JT808TrafficInfoDto>> GetUdpTraffic();
        /// <summary>
        /// 获取Udp包计数器
        /// </summary>
        /// <returns></returns>
        [HttpGet("Udp/GetAtomicCounter")]
        ITask<JT808ResultDto<JT808AtomicCounterDto>> GetUdpAtomicCounter();
        #endregion

        #region 公共部分
        /// <summary>
        /// 获取当前系统进程使用率
        /// </summary>
        /// <returns></returns>
        [HttpGet("SystemCollect/Get")]
        ITask<JT808ResultDto<JT808SystemCollectInfoDto>> GetSystemCollect();
        #endregion
    }
}
