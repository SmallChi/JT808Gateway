using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.Gateway.SimpleQueueNotification.Configs;
using JT808.Gateway.SimpleQueueNotification.Hubs;
using JT808.Gateway.SimpleQueueNotification.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JT808.Protocol;
using JT808.Gateway.Kafka;
using JT808.Gateway.MsgIdHandler;
using JT808.Gateway.SimpleQueueNotification.Impl;

namespace JT808.Gateway.SimpleQueueNotification
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                           .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((configure) => {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Trace);
            });
            services.AddSignalR();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                //builder.AllowAnyOrigin()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowAnyOrigin();
                builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(Configuration.GetSection("CorsHosts").Get<List<string>>().ToArray())
                        .AllowCredentials();
            }));
            services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));
            services.AddJT808Configure()
                    .AddClientKafka()
                    .AddMsgConsumer(Configuration)
                    .AddMsgIdHandler<JT808MsgIdHandlerImpl>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseFileServer();
            app.UseRouting();
            app.UseJT808JwtVerify();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<JT808MsgHub>("/JT808MsgHub");
            });
        }
    }
}
