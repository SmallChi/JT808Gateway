using JT808.Gateway.Abstractions;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.WebApiClientTool
{
    /// <summary>
    /// 
    /// </summary>
    public static class JT808HttpClientExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="webapiUri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IServiceCollection AddJT808WebApiClientTool(this IServiceCollection serviceDescriptors, Uri webapiUri,string token)
        {
            serviceDescriptors.AddHttpClient("JT808WebApiClientTool", c =>
            {
                c.DefaultRequestHeaders.Add("token", token);
                c.BaseAddress = webapiUri;
                c.Timeout = TimeSpan.FromSeconds(3);
            })
            .AddTypedClient<JT808HttpClient>();
            return serviceDescriptors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJT808WebApiClientTool(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddHttpClient("JT808WebApiClientTool", c =>
            {
                c.DefaultRequestHeaders.Add("token", configuration.GetSection("JT808WebApiClientToolConfig:Token").Get<string>());
                c.BaseAddress = new Uri(configuration.GetSection("JT808WebApiClientToolConfig:Uri").Get<string>());
                c.Timeout = TimeSpan.FromSeconds(3);
            })
            .AddTypedClient<JT808HttpClient>();
            return serviceDescriptors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808Builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808Builder AddWebApiClientTool(this IJT808Builder jT808Builder, IConfiguration configuration)
        {
            jT808Builder.Services.AddHttpClient("JT808WebApiClientTool", c =>
            {
                c.DefaultRequestHeaders.Add("token", configuration.GetSection("JT808WebApiClientToolConfig:Token").Get<string>());
                c.BaseAddress = new Uri(configuration.GetSection("JT808WebApiClientToolConfig:Uri").Get<string>());
                c.Timeout = TimeSpan.FromSeconds(3);
            })
            .AddTypedClient<JT808HttpClient>();
            return jT808Builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808Builder"></param>
        /// <param name="webapiUri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IJT808Builder AddWebApiClientTool(this IJT808Builder jT808Builder, Uri webapiUri, string token)
        {
            jT808Builder.Services.AddHttpClient("JT808WebApiClientTool", c =>
            {
                c.DefaultRequestHeaders.Add("token", token);
                c.BaseAddress = webapiUri;
                c.Timeout = TimeSpan.FromSeconds(3);
            })
             .AddTypedClient<JT808HttpClient>();
            return jT808Builder;
        }
    }
}
