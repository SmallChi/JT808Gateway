using DotNetty.Buffers;
using DotNetty.Codecs.Http;
using DotNetty.Common;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Handlers
{
    /// <summary>
    /// jt808 webapi服务
    /// 请求量不大，只支持JSON格式
    /// ref: dotnetty HttpServer
    /// </summary>
    internal class JT808WebAPIServerHandler : ChannelHandlerAdapter
    {
        private static readonly ThreadLocalCache Cache = new ThreadLocalCache();

        sealed class ThreadLocalCache : FastThreadLocal<AsciiString>
        {
            protected override AsciiString GetInitialValue()
            {
                DateTime dateTime = DateTime.UtcNow;
                return AsciiString.Cached($"{dateTime.DayOfWeek}, {dateTime:dd MMM yyyy HH:mm:ss z}");
            }
        }

        private static readonly AsciiString TypeJson = AsciiString.Cached("application/json");
        private static readonly AsciiString ServerName = AsciiString.Cached("JT808WebAPINetty");
        private static readonly AsciiString ContentTypeEntity = HttpHeaderNames.ContentType;
        private static readonly AsciiString DateEntity = HttpHeaderNames.Date;
        private static readonly AsciiString ContentLengthEntity = HttpHeaderNames.ContentLength;
        private static readonly AsciiString ServerEntity = HttpHeaderNames.Server;

        volatile ICharSequence date = Cache.Value;

        private readonly ILogger<JT808WebAPIServerHandler> logger;

        private readonly IJT808SessionService jT808SessionService;

        private readonly IJT808UnificationSendService jT808UnificationSendService;

        public JT808WebAPIServerHandler(
            IJT808SessionService jT808SessionService,
            IJT808UnificationSendService  jT808UnificationSendService,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionService = jT808SessionService;
            this.jT808UnificationSendService = jT808UnificationSendService;
            logger = loggerFactory.CreateLogger<JT808WebAPIServerHandler>();
        }

        public override void ChannelRead(IChannelHandlerContext ctx, object message)
        {
            if (message is IHttpRequest request)
            {
                try
                {
                    Process(ctx, request);
                }
                finally
                {
                    ReferenceCountUtil.Release(message);
                }
            }
            else
            {
                ctx.FireChannelRead(message);
            }
        }

        private void Process(IChannelHandlerContext ctx, IHttpRequest request)
        {
            string uri = request.Uri;
            //switch (uri)
            //{
            //    //case "/json":
            //    //    byte[] json = Encoding.UTF8.GetBytes(NewMessage().ToJsonFormat());
            //    //    this.WriteResponse(ctx, Unpooled.WrappedBuffer(json), TypeJson, JsonClheaderValue);
            //    //    break;
            //    default:
            //        var response = new DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.NotFound, Unpooled.Empty, false);
            //        ctx.WriteAndFlushAsync(response);
            //        ctx.CloseAsync();
            //        break;
            //}
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808DefaultResultDto()));
            this.WriteResponse(ctx, Unpooled.WrappedBuffer(json), TypeJson, json.Length);
        }

        private void WriteResponse(IChannelHandlerContext ctx, IByteBuffer buf, ICharSequence contentType, int contentLength)
        {
            // Build the response object.
            var response = new DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.OK, buf, false);
            HttpHeaders headers = response.Headers;
            headers.Set(ContentTypeEntity, contentType);
            headers.Set(ServerEntity, ServerName);
            headers.Set(DateEntity, this.date);
            headers.Set(ContentLengthEntity, contentLength);
            // Close the non-keep-alive connection after the write operation is done.
            ctx.WriteAsync(response);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string channelId = context.Channel.Id.AsShortText();
            logger.LogError(exception, $"{channelId} {exception.Message}");
            context.CloseAsync();
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
    }
}
