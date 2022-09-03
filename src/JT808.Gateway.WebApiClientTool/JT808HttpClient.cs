using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Dtos;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.Gateway.WebApiClientTool
{
    /// <summary>
    /// 
    /// </summary>
    public class JT808HttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        public JT808HttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        /// <summary>
        /// 统一下发信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> UnificationSend(JT808UnificationSendRequestDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.UnificationSend, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<bool>>();
            return value;
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<List<JT808TcpSessionInfoDto>>> GetTcpSessionAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808GatewayConstants.JT808WebApiRouteTable.SessionTcpGetAll);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<List<JT808TcpSessionInfoDto>>>();
            return value;
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>> SessionTcpByPage(int pageIndex=0,int pageSize=10)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{JT808GatewayConstants.JT808WebApiRouteTable.SessionTcpByPage}?pageIndex={pageIndex}&pageSize={pageSize}");
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>>();
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808TcpSessionInfoDto>> QueryTcpSessionByTerminalPhoneNo(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.QueryTcpSessionByTerminalPhoneNo, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<JT808TcpSessionInfoDto>>();
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> RemoveTcpByTerminalPhoneNo(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<bool>>();
            return value;
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<List<JT808UdpSessionInfoDto>>> GetUdpSessionAll()
        {
            var response = await HttpClient.GetAsync(JT808GatewayConstants.JT808WebApiRouteTable.SessionUdpGetAll);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<List<JT808UdpSessionInfoDto>>>();
            return value;
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>> SessionUdpByPage(int pageIndex = 0, int pageSize = 10)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{JT808GatewayConstants.JT808WebApiRouteTable.SessionUdpByPage}?pageIndex={pageIndex}&pageSize={pageSize}");
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>>();
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808UdpSessionInfoDto>> QueryUdpSessionByTerminalPhoneNo(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.QueryUdpSessionByTerminalPhoneNo, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<JT808UdpSessionInfoDto>>();
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> RemoveUdpByTerminalPhoneNo(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.RemoveUdpByTerminalPhoneNo,parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<bool>>();
            return value;
        }

        /// <summary>
        /// SIM卡黑名单服务-将对应SIM号加入黑名单
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> BlacklistAdd(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistAdd, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<bool>>();
            return value;
        }

        /// <summary>
        /// SIM卡黑名单服务-将对应SIM号移除黑名单
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> BlacklistRemove(JT808TerminalPhoneNoDto parameter)
        {
            var response = await HttpClient.PostAsJsonAsync(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistRemove, parameter);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<bool>>();
            return value;
        }

        /// <summary>
        /// SIM卡黑名单服务-获取所有sim的黑名单列表
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<List<string>>> GetBlacklistAll()
        {
            var response = await HttpClient.GetAsync(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistGet);
            response.EnsureSuccessStatusCode();
            var value = await response.Content.ReadFromJsonAsync<JT808ResultDto<List<string>>>();
            return value;
        }
    }
}
