using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JT808.Protocol;
using JT808.DotNetty.Client;
using JT808.DotNetty.CleintBenchmark.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using JT808.DotNetty.CleintBenchmark.Services;

namespace JT808.DotNetty.CleintBenchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseCors("Domain");
                    app.UseStaticFiles();
                    app.UseDefaultFiles("/index.html");
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                })
                .ConfigureServices((hostContext, services) => 
                {
                    services.AddControllers();
                    services.AddCors(options =>
                        options.AddPolicy("Domain", builder =>
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowAnyOrigin()));

                });
            })
            .ConfigureLogging((context, logging) =>
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    NLog.LogManager.LoadConfiguration("Configs/nlog.unix.config");
                }
                else
                {
                    NLog.LogManager.LoadConfiguration("Configs/nlog.win.config");
                }
                logging.AddNLog();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ClientBenchmarkOptions>(hostContext.Configuration.GetSection("ClientBenchmarkOptions"));
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddJT808Configure()
                        .AddJT808Client();
                services.AddHostedService<CleintBenchmarkHostedService>();
            });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
