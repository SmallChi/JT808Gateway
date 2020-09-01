using JT808.Gateway.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JT808.Gateway.Abstractions
{
    public abstract class JT808MsgIdHttpHandlerBase
    {
        public Dictionary<string, Func<string,byte[]>> HandlerDict { get; }

        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        protected JT808MsgIdHttpHandlerBase()
        {
            HandlerDict = new Dictionary<string, Func<string, byte[]>>();
        }

        protected void CreateRoute(string url, Func<string, byte[]> func)
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

        protected byte[] CreateHttpResponse(dynamic dynamicObject)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(dynamicObject);
            return data;
        }

        public byte[] DefaultHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808DefaultResultDto());
            return json;
        }

        public byte[] EmptyHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.Empty,
                Message = "内容为空",
                Data = "Content Empty"
            });
            return json;
        }

        public byte[] NotFoundHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.NotFound,
                Message = "没有该服务",
                Data = "没有该服务"
            });
            return json;
        }

        public byte[] AuthFailHttpResponse()
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.AuthFail,
                Message = "token认证失败",
                Data = "token认证失败"
            });
            return json;
        }

        public byte[] ErrorHttpResponse(Exception ex)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.Error,
                Message = ex.StackTrace,
                Data = ex.Message
            });
            return json;
        }
    }
}
