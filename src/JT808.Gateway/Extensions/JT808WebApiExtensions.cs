using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JT808.Gateway.Extensions
{
    public static class JT808WebApiExtensions
    {
        public static IServiceCollection AddJT808Cors(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.AddCors(options =>
                 options.AddPolicy("jt808", builder =>
                 builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(o => true)));
            return serviceDescriptors;
        }

        public static IApplicationBuilder UseJT808Cors(this IApplicationBuilder app)
        {
            app.UseCors("jt808");
            return app;
        }
    }
}
