using JT808.DotNetty.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace JT808.DotNetty.Dashbord.Internal
{
    public interface IJT808DotNettyWebApi : IHttpApi
    {
        /// <summary>
        /// 基于Tcp的统一下发信息
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        ITask<JT808ResultDto<bool>> UnificationTcpSend([Uri]string uri, [FormContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
        /// <summary>
        /// /基于Udp的统一下发信息
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        ITask<JT808ResultDto<bool>> UnificationUdpSend([Uri]string uri, [FormContent]JT808UnificationSendRequestDto jT808UnificationSendRequestDto);
    }
}
