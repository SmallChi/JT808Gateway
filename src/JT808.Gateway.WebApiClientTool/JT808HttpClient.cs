using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Dtos;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net.Http;
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
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> UnificationSend(JT808UnificationSendRequestDto jT808UnificationSendRequestDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808GatewayConstants.JT808WebApiRouteTable.UnificationSend);
            request.Content = new StringContent(JsonSerializer.Serialize(jT808UnificationSendRequestDto));
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<bool>>(data);
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
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<List<JT808TcpSessionInfoDto>>>(data);
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话信息
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808TcpSessionInfoDto>> QueryTcpSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808GatewayConstants.JT808WebApiRouteTable.QueryTcpSessionByTerminalPhoneNo);
            request.Content = new StringContent(terminalPhoneNo);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<JT808TcpSessionInfoDto>>(data);
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> RemoveTcpByTerminalPhoneNo(string terminalPhoneNo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808GatewayConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo);
            request.Content = new StringContent(terminalPhoneNo);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<bool>>(data);
            return value;
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<List<JT808UdpSessionInfoDto>>> GetUdpSessionAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808GatewayConstants.JT808WebApiRouteTable.SessionUdpGetAll);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<List<JT808UdpSessionInfoDto>>>(data);
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话信息
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<JT808UdpSessionInfoDto>> QueryUdpSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808GatewayConstants.JT808WebApiRouteTable.QueryUdpSessionByTerminalPhoneNo);
            request.Content = new StringContent(terminalPhoneNo);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<JT808UdpSessionInfoDto>>(data);
            return value;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        public async ValueTask<JT808ResultDto<bool>> RemoveUdpByTerminalPhoneNo(string terminalPhoneNo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808GatewayConstants.JT808WebApiRouteTable.RemoveUdpByTerminalPhoneNo);
            request.Content = new StringContent(terminalPhoneNo);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStreamAsync();
            var value = await JsonSerializer.DeserializeAsync<JT808ResultDto<bool>>(data);
            return value;
        }
    }
}
