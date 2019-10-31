using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace JT808.DotNetty.WebApi.Handlers
{
    /// <summary>
    /// jt808 webapi服务
    /// 请求量不大，只支持JSON格式并且只支持post发数据
    /// ref: dotnetty HttpServer
    /// </summary>
    internal class JT808WebAPIServerHandler : SimpleChannelInboundHandler<IFullHttpRequest>
    {
        private static readonly AsciiString TypeJson = AsciiString.Cached("application/json");
        private static readonly AsciiString ServerName = AsciiString.Cached("JT808WebAPINetty");
        private static readonly AsciiString ContentTypeEntity = HttpHeaderNames.ContentType;
        private static readonly AsciiString DateEntity = HttpHeaderNames.Date;
        private static readonly AsciiString ContentLengthEntity = HttpHeaderNames.ContentLength;
        private static readonly AsciiString ServerEntity = HttpHeaderNames.Server;
        private readonly JT808MsgIdHttpHandlerBase  jT808MsgIdHttpHandlerBase;
        private readonly ILogger<JT808WebAPIServerHandler> logger;
        private readonly IJT808WebApiAuthorization jT808WebApiAuthorization;
        public JT808WebAPIServerHandler(
            IJT808WebApiAuthorization jT808WebApiAuthorization,
            JT808MsgIdHttpHandlerBase jT808MsgIdHttpHandlerBase,
            ILoggerFactory loggerFactory)
        {
            this.jT808WebApiAuthorization = jT808WebApiAuthorization;
            this.jT808MsgIdHttpHandlerBase = jT808MsgIdHttpHandlerBase;
            logger = loggerFactory.CreateLogger<JT808WebAPIServerHandler>();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IFullHttpRequest msg)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Uri:{msg.Uri}");
                logger.LogDebug($"Content:{msg.Content.ToString(Encoding.UTF8)}");
            }
            JT808HttpResponse jT808HttpResponse = null;
            if (!jT808WebApiAuthorization.Authorization(msg, out var principal))
            {
                jT808HttpResponse = jT808MsgIdHttpHandlerBase.AuthFailHttpResponse();
            }
            else
            {
                var uriSpan = msg.Uri.AsSpan();
                var index = uriSpan.IndexOf('?');
                string uri = msg.Uri;
                if (index > 0)
                {
                    uri = uriSpan.Slice(0, index).ToString();
                }
                if (jT808MsgIdHttpHandlerBase.HandlerDict.TryGetValue(uri, out var funcHandler))
                {
                    jT808HttpResponse = funcHandler(new JT808HttpRequest() { Json = msg.Content.ToString(Encoding.UTF8) });
                }
                else
                {
                    jT808HttpResponse = jT808MsgIdHttpHandlerBase.NotFoundHttpResponse();
                }
            }
            if (jT808HttpResponse != null)
            {
                WriteResponse(ctx, Unpooled.WrappedBuffer(jT808HttpResponse.Data), TypeJson, jT808HttpResponse.Data.Length);
            }            
        }

        private void WriteResponse(IChannelHandlerContext ctx, IByteBuffer buf, ICharSequence contentType, int contentLength)
        {
            // Build the response object.
            var response = new DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.OK, buf, false);
            HttpHeaders headers = response.Headers;
            headers.Set(ContentTypeEntity, contentType);
            headers.Set(ServerEntity, ServerName);
            headers.Set(DateEntity, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            headers.Set(ContentLengthEntity, contentLength);
            // Close the non-keep-alive connection after the write operation is done.
            ctx.WriteAndFlushAsync(response);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            WriteResponse(context, Unpooled.WrappedBuffer(jT808MsgIdHttpHandlerBase.ErrorHttpResponse(exception).Data), TypeJson, jT808MsgIdHttpHandlerBase.ErrorHttpResponse(exception).Data.Length);
            logger.LogError(exception, exception.Message);
            context.CloseAsync();
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
    }
}
