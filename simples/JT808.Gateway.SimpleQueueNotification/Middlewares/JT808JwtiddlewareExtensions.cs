using Microsoft.AspNetCore.Builder;


namespace JT808.Gateway.SimpleQueueNotification.Middlewares
{
    public static class JT808JwtiddlewareExtensions
    {
        public static IApplicationBuilder UseJT808JwtVerify(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JT808JwtMiddlewares>();
        }
    }
}