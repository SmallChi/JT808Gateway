using System;
using System.Threading.Tasks;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.SessionNotice;
using JT808.Gateway.SimpleServer.Impl;
using JT808.Gateway.SimpleServer.Services;
using JT808.Gateway.Transmit;
using JT808.Protocol;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway.SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                //使用内存队列实现会话通知
                .AddSingleton<JT808SessionService>()
                .AddSingleton<IJT808SessionProducer, JT808SessionProducer>()
                .AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>()
                .AddJT808Configure()
                .AddGateway(builder.Configuration)
                .AddMessageHandler<JT808MessageHandlerImpl>()
                .AddMsgLogging<JT808MsgLogging>()
                .AddSessionNotice()
                .AddTransmit(builder.Configuration)
                .AddTcp()
                .AddUdp()
                .Builder();

            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseRouting();
            app.MapControllers();
            app.Run();
        }
    }
}
