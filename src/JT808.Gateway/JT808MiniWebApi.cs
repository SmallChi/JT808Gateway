using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Http;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Dtos;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using JT808.Gateway.Session;
using JT808.Gateway.Services;
using System;

namespace JT808.Gateway
{
    /// <summary>
    /// 
    /// </summary>
    public static class JT808MiniWebApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void UseJT808MiniWebApi(this WebApplication app)
        {
            //CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.UnificationSend, UnificationSend);
            //CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistAdd, BlacklistAdd);
            //CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistRemove, BlacklistRemove);
            //CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.BlacklistGet, QueryBlacklist);
            app.MapPost(JT808GatewayConstants.JT808WebApiRouteTable.UnificationSend,async context =>
            {
                JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
                JT808SessionManager SessionManager = context.RequestServices.GetRequiredService<JT808SessionManager>();
                JT808BlacklistManager BlacklistManager = context.RequestServices.GetRequiredService<JT808BlacklistManager>();
                try
                {
                    JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonSerializer.Deserialize<JT808UnificationSendRequestDto>(context.Request.Body);
                    resultDto.Data = await SessionManager.TrySendByTerminalPhoneNoAsync(jT808UnificationSendRequestDto.TerminalPhoneNo,Convert.FromHexString(jT808UnificationSendRequestDto.HexData));
                    resultDto.Code = JT808ResultCode.Ok;
                }
                catch (Exception ex)
                {
                    resultDto.Data = false;
                    resultDto.Code = JT808ResultCode.Error;
                    resultDto.Message = ex.StackTrace;
                }
                await context.Response.WriteAsJsonAsync(resultDto);
            });
        }
    }
}
