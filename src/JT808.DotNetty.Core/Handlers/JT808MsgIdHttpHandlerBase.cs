using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core.Handlers
{
    /// <summary>
    /// 基于webapi http模式抽象消息处理业务
    /// 自定义消息处理业务
    /// 注意:
    /// 1.ConfigureServices:
    /// services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHttpHandlerBase),typeof(JT808MsgIdCustomHttpHandlerImpl),ServiceLifetime.Singleton));
    /// 2.解析具体的消息体，具体消息调用具体的JT808Serializer.Deserialize<T>
    /// </summary>
    public abstract class JT808MsgIdHttpHandlerBase
    {
        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        protected JT808MsgIdHttpHandlerBase()
        {
            HandlerDict = new Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>>();
        }

        protected void CreateRoute(string url, Func<JT808HttpRequest, JT808HttpResponse> func)
        {
            if (!HandlerDict.ContainsKey(url))
            {
                HandlerDict.Add(url, func);
            }
            else
            {
                // 替换
                HandlerDict[url] = func;
            }
        }

        public Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>> HandlerDict { get; }

        protected JT808HttpResponse CreateJT808HttpResponse(dynamic dynamicObject)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(dynamicObject);
            return new JT808HttpResponse()
            {
                Data = data
            };
        }

        public JT808HttpResponse DefaultHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808DefaultResultDto());
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse EmptyHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.Empty,
                Message = "内容为空",
                Data = "Content Empty"
            });
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse NotFoundHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.NotFound,
                Message = "没有该服务",
                Data = "没有该服务"
            });
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse AuthFailHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.AuthFail,
                Message = "token认证失败",
                Data = "token认证失败"
            });
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse ErrorHttpResponse(Exception ex)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.Error,
                Message = ex.StackTrace,
                Data = ex.Message
            });
            return new JT808HttpResponse(json);
        }
    }
}
