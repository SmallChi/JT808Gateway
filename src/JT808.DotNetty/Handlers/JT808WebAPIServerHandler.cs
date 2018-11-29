using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Internal;
using JT808.DotNetty.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace JT808.DotNetty.Handlers
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
        private readonly JT808WebAPIService jT808WebAPIService;
        private readonly ILogger<JT808WebAPIServerHandler> logger;

        public JT808WebAPIServerHandler(
            JT808WebAPIService jT808WebAPIService,
            ILoggerFactory loggerFactory)
        {
            this.jT808WebAPIService = jT808WebAPIService;
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
            if (jT808WebAPIService.HandlerDict.TryGetValue(msg.Uri,out var funcHandler))
            {
                jT808HttpResponse = funcHandler( new JT808HttpRequest() { Json = msg.Content.ToString(Encoding.UTF8)});
            }
            else
            {
                jT808HttpResponse = jT808WebAPIService.NotFoundHttpResponse();
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
            ctx.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            WriteResponse(context, Unpooled.WrappedBuffer(jT808WebAPIService.ErrorHttpResponse(exception).Data), TypeJson, jT808WebAPIService.ErrorHttpResponse(exception).Data.Length);
            logger.LogError(exception, exception.Message);
            context.CloseAsync();
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
    }
}
