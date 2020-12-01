using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Extensions
{
    public static class JT808HttpContextExtensions
    {
        private const string jsonType = "application/json";

        public static async ValueTask Http401(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("auth error");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = jsonType;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b, CancellationToken.None);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask Http400(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes($"sim and channel parameter required.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b, CancellationToken.None);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void Http404(this HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void Http405(this HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask Http500(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("inner error");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            await output.WriteAsync(b,CancellationToken.None);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask HttpSend(this HttpListenerContext context, ReadOnlyMemory<byte> buffer)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.KeepAlive = true;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static async ValueTask HttpSend(this HttpListenerContext context, string json)
        {
            byte[] b = Encoding.UTF8.GetBytes(json);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.KeepAlive = true;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            await context.Response.OutputStream.WriteAsync(b);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }
    }
}
