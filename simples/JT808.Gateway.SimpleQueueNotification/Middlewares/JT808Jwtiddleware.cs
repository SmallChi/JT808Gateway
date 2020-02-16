using JT808.Gateway.SimpleQueueNotification.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.Gateway.SimpleQueueNotification.Middlewares
{
    public class JT808JwtMiddlewares
    {
        private readonly RequestDelegate next;

        private readonly ILogger logger;

        IOptionsMonitor<AuthOptions> authOptionsMonitor;

        public JT808JwtMiddlewares(RequestDelegate next,
            IOptionsMonitor<AuthOptions> authOptionsMonitor,
            ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.authOptionsMonitor = authOptionsMonitor;
            logger = loggerFactory.CreateLogger<JT808JwtMiddlewares>();
        }

        public async Task Invoke(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(getIp(context));
            sb.Append(",");
            sb.Append(getBrowser(context));
            sb.Append(",");
            if (context.Request.Query.TryGetValue("access_token", out var token))
            {
                if (token == "")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = (int)HttpStatusCode.Unauthorized, Msg = HttpStatusCode.Unauthorized.ToString() }));
                    return;
                }
                else
                {
                    sb.Append(token);
                }
                try
                {
                    // 去认证服务器请求结果
                    // 如果超级Token有值就不去jwt认证
                    if (!string.IsNullOrEmpty(authOptionsMonitor.CurrentValue.SuperToken))
                    {
                        if(authOptionsMonitor.CurrentValue.SuperToken != token)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = (int)HttpStatusCode.Unauthorized, Msg = HttpStatusCode.Unauthorized.ToString() }));
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = (int)HttpStatusCode.ExpectationFailed, Msg = HttpStatusCode.ExpectationFailed.ToString() }));
                    return;
                }
                if(logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug($"认证成功:{sb.ToString()}");
                await next(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { Code = (int)HttpStatusCode.Unauthorized, Msg = HttpStatusCode.Unauthorized.ToString() }));
            }
        }

        private string getIp(HttpContext context)
        {
            Microsoft.Extensions.Primitives.StringValues ips;
            if (context.Request.Headers.TryGetValue("X-Real-IP", out ips))
            {
                return ips.FirstOrDefault() ?? "";
            }
            else
            {
                return context.Connection.RemoteIpAddress?.ToString() ?? "";
            }
        }

        private static string getBrowser(HttpContext context)
        {
            return context.Request.Headers["User-Agent"].FirstOrDefault();
        }
    }
}