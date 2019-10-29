using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Dtos;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.DotNetty.WebApiClientTool
{
    public class JT808HttpClient
    {
        public HttpClient HttpClient { get; }
        public JT808HttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public JT808ResultDto<List<JT808TcpSessionInfoDto>> GetTcpSessionAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll);
            var response =  HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value = JsonSerializer.Deserialize<JT808ResultDto<List<JT808TcpSessionInfoDto>>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        public JT808ResultDto<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808NettyConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo);
            request.Content = new StringContent(terminalPhoneNo);
            var response =  HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value =  JsonSerializer.Deserialize<JT808ResultDto<bool>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
        /// <summary>
        /// 统一下发信息
        /// </summary>
        /// <param name="jT808UnificationSendRequestDto"></param>
        /// <returns></returns>
        public JT808ResultDto<bool> UnificationSend(JT808UnificationSendRequestDto jT808UnificationSendRequestDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, JT808NettyConstants.JT808WebApiRouteTable.UnificationSend);
            request.Content = new StringContent(JsonSerializer.Serialize(jT808UnificationSendRequestDto));
            var response = HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value =  JsonSerializer.Deserialize<JT808ResultDto<bool>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
        /// <summary>
        /// 获取Tcp包计数器
        /// </summary>
        /// <returns></returns>
        public JT808ResultDto<JT808AtomicCounterDto> GetTcpAtomicCounter()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808NettyConstants.JT808WebApiRouteTable.GetTcpAtomicCounter);
            var response =  HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value = JsonSerializer.Deserialize<JT808ResultDto<JT808AtomicCounterDto>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <returns></returns>
        public  JT808ResultDto<List<JT808UdpSessionInfoDto>> GetUdpSessionAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808NettyConstants.JT808WebApiRouteTable.SessionUdpGetAll);
            var response = HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value = JsonSerializer.Deserialize<JT808ResultDto<List<JT808UdpSessionInfoDto>>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
        /// <summary>
        /// 获取Udp包计数器
        /// </summary>
        /// <returns></returns>
        public JT808ResultDto<JT808AtomicCounterDto> GetUdpAtomicCounter()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, JT808NettyConstants.JT808WebApiRouteTable.GetUdpAtomicCounter);
            var response =  HttpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var value = JsonSerializer.Deserialize<JT808ResultDto<JT808AtomicCounterDto>>(response.Content.ReadAsByteArrayAsync().Result);
            return value;
        }
    }
}
