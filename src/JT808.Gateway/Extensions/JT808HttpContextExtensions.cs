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

        public static void Http204(this HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            context.Response.KeepAlive = false;
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void Http401(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("auth error");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = jsonType;
            context.Response.KeepAlive = false;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            output.Write(b, 0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void Http400(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes($"sim and channel parameter required.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            output.Write(b, 0, b.Length);
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

        public static void Http500(this HttpListenerContext context)
        {
            byte[] b = Encoding.UTF8.GetBytes("inner error");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            var output = context.Response.OutputStream;
            output.Write(b,0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void HttpSend(this HttpListenerContext context, ReadOnlyMemory<byte> buffer)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer.ToArray(),0, buffer.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        public static void HttpSend(this HttpListenerContext context, string json)
        {
            byte[] b = Encoding.UTF8.GetBytes(json);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.KeepAlive = false;
            context.Response.ContentType = jsonType;
            context.Response.ContentLength64 = b.Length;
            context.Response.OutputStream.Write(b,0, b.Length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }
    }
}
