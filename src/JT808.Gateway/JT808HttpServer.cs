using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Extensions;
using JT808.Gateway.Handlers;
using JT808.Gateway.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway
{
    public class JT808HttpServer : IHostedService
    {
        private readonly ILogger Logger;

        private readonly JT808Configuration Configuration;

        private readonly IJT808Authorization authorization;

        private HttpListener listener;

        private readonly JT808MsgIdDefaultWebApiHandler MsgIdDefaultWebApiHandler;

        public JT808HttpServer(
            JT808MsgIdDefaultWebApiHandler jT808MsgIdDefaultWebApiHandler,
            IOptions<JT808Configuration> jT808ConfigurationAccessor,
            IJT808Authorization authorization,
            ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<JT808HttpServer>();
            Configuration = jT808ConfigurationAccessor.Value;
            MsgIdDefaultWebApiHandler = jT808MsgIdDefaultWebApiHandler;
            this.authorization = authorization;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!HttpListener.IsSupported)
            {
                Logger.LogWarning("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return Task.CompletedTask;
            }
            listener = new HttpListener();
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            try
            {
                listener.Prefixes.Add($"http://*:{Configuration.WebApiPort}/");
                listener.Start();
            }
            catch (System.Net.HttpListenerException ex)
            {
                Logger.LogWarning(ex, $"{ex.Message}:使用cmd命令[netsh http add urlacl url=http://*:{Configuration.WebApiPort}/ user=Everyone]");
            }
            Logger.LogInformation($"JT808 Http Server start at {IPAddress.Any}:{Configuration.WebApiPort}.");
            Task.Factory.StartNew(async () =>
            {
                while (listener.IsListening)
                {
                    var context = await listener.GetContextAsync();
                    try
                    {
                        if (context.Request.RawUrl.StartsWith("/favicon.ico"))
                        {
                            context.Http404();
                            continue;
                        }
                        if (authorization.Authorization(context, out var principal))
                        {
                            await ProcessRequestAsync(context, principal);
                        }
                        else
                        {
                            await context.Http401();
                        }
                    }
                    catch (AggregateException ex)
                    {
                        await context.Http500();
                        Logger.LogError(ex, ex.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        await context.Http500();
                        Logger.LogError(ex, ex.StackTrace);
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        private async ValueTask ProcessRequestAsync(HttpListenerContext context, IPrincipal principal)
        {
            var router = MsgIdDefaultWebApiHandler.HandlerDict.FirstOrDefault(f => context.Request.RawUrl.StartsWith(f.Key));
            if (router.Key == null)
            {
                context.Http404();
                return;
            }
            if(context.Request.HttpMethod == HttpMethod.Get.Method)
            {
                var index = context.Request.Url.AbsoluteUri.IndexOf('?');
                if (index > 0)
                {
                    var uriParamStr = context.Request.Url.AbsoluteUri[(index + 1)..].ToString();
                    await context.HttpSend(router.Value(uriParamStr));
                }
                else
                {
                    await context.HttpSend(router.Value(""));
                }
            }
            else if(context.Request.HttpMethod == HttpMethod.Post.Method)
            {
                string json = string.Empty;
                using (var reader = new StreamReader(context.Request.InputStream,context.Request.ContentEncoding))
                {
                    json = reader.ReadToEnd();
                }
                await context.HttpSend(router.Value(json));
            }
            else
            {
                context.Http405();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                listener.Stop();
            }
            catch (System.ObjectDisposedException ex)
            {
                Logger.LogError(ex, "");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "");
            }
            return Task.CompletedTask;
        }
    }
}
