using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using JT808.DotNetty.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty
{
    internal class JT808WebAPIService
    {
        public Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>> HandlerDict { get; protected set; }

        private const string RouteTablePrefix = "/jt808api";

        private const string sessionRoutePrefix = "Session";

        private readonly IJT808SessionService jT808SessionService;

        private readonly IJT808UnificationSendService jT808UnificationSendService;

        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        protected JT808WebAPIService(
            IJT808SessionService jT808SessionService,
            IJT808UnificationSendService jT808UnificationSendService)
        {
            this.jT808SessionService = jT808SessionService;
            this.jT808UnificationSendService = jT808UnificationSendService;
            HandlerDict = new Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>>
            {
                {$"{RouteTablePrefix}/UnificationSend", UnificationSend},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetRealLinkCount", GetRealLinkCount},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetRelevanceLinkCount", GetRelevanceLinkCount},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetRealAll", GetRealAll},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetRelevanceAll", GetRelevanceAll},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/RemoveByChannelId", RemoveByChannelId},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/RemoveByTerminalPhoneNo", RemoveByTerminalPhoneNo},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetByChannelId", GetByChannelId},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetByTerminalPhoneNo", GetByTerminalPhoneNo},
            };
        }

        /// <summary>
        /// 统一下发信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse UnificationSend(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonConvert.DeserializeObject<JT808UnificationSendRequestDto>(request.Json);
            var result = jT808UnificationSendService.Send(jT808UnificationSendRequestDto.TerminalPhoneNo, jT808UnificationSendRequestDto.Data);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-获取真实连接数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetRealLinkCount(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetRealAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-获取设备相关连的连接数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetRelevanceLinkCount(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetRelevanceLinkCount();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-获取实际会话集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetRealAll(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetRealAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-获取设备相关连会话集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetRelevanceAll(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetRelevanceAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过通道Id移除对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveByChannelId(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808SessionService.RemoveByChannelId(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808SessionService.RemoveByTerminalPhoneNo(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过通道Id获取会话信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetByChannelId(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808SessionService.GetByChannelId(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过设备终端号获取会话信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808SessionService.GetByTerminalPhoneNo(request.Json);
            return CreateJT808HttpResponse(result);
        }

        private JT808HttpResponse CreateJT808HttpResponse(dynamic dynamicObject)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dynamicObject));
            return new JT808HttpResponse()
            {
                Data = data
            };
        }

        public JT808HttpResponse DefaultHttpResponse()
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808DefaultResultDto()));
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse EmptyHttpResponse()
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808ResultDto<string>()
            {
                 Code=201,
                 Message="内容为空",
                 Data="Content Empty"
            }));
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse NotFoundHttpResponse()
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808ResultDto<string>()
            {
                 Code=404,
                 Message="没有该服务",
                 Data= "没有该服务"
            }));
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse ErrorHttpResponse(Exception ex)
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808ResultDto<string>()
            {
                Code = 500,
                Message = JsonConvert.SerializeObject(ex),
                Data= ex.Message
            }));
            return new JT808HttpResponse(json);
        }
    }
}
