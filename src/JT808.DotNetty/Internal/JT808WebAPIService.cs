using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using JT808.DotNetty.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// JT808 WebApi 业务服务
    /// </summary>
    internal class JT808WebAPIService
    {
        public Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>> HandlerDict { get; protected set; }

        private const string RouteTablePrefix = "/jt808api";

        private const string sessionRoutePrefix = "Session";

        private const string sourcePackagePrefix = "SourcePackage";

        private const string transmitPrefix = "Transmit";
        
        private readonly IJT808SessionService jT808SessionService;

        private readonly IJT808UnificationSendService jT808UnificationSendService;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly JT808SourcePackageChannelService jT808SourcePackageChannelService;

        private readonly JT808TransmitAddressFilterService jT808TransmitAddressFilterService;

        /// <summary>
        /// 初始化消息处理业务
        /// </summary>
        public JT808WebAPIService(
            JT808AtomicCounterService jT808AtomicCounterService,
            JT808SourcePackageChannelService jT808SourcePackageChannelService,
            JT808TransmitAddressFilterService jT808TransmitAddressFilterService,
            IJT808SessionService jT808SessionService,
            IJT808UnificationSendService jT808UnificationSendService)
        {
            this.jT808AtomicCounterService = jT808AtomicCounterService;
            this.jT808SourcePackageChannelService = jT808SourcePackageChannelService;
            this.jT808TransmitAddressFilterService = jT808TransmitAddressFilterService;
            this.jT808SessionService = jT808SessionService;
            this.jT808UnificationSendService = jT808UnificationSendService;
            HandlerDict = new Dictionary<string, Func<JT808HttpRequest, JT808HttpResponse>>
            {
                {$"{RouteTablePrefix}/UnificationSend", UnificationSend},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/GetAll", GetSessionAll},
                {$"{RouteTablePrefix}/{sessionRoutePrefix}/RemoveByTerminalPhoneNo", RemoveByTerminalPhoneNo},
                {$"{RouteTablePrefix}/GetAtomicCounter", GetAtomicCounter},
                {$"{RouteTablePrefix}/{sourcePackagePrefix}/Add", AddSourcePackageAddress},
                {$"{RouteTablePrefix}/{sourcePackagePrefix}/Remove", RemoveSourcePackageAddress},
                {$"{RouteTablePrefix}/{sourcePackagePrefix}/GetAll", GetSourcePackageAll},
                {$"{RouteTablePrefix}/{transmitPrefix}/Add", AddTransmitAddress},
                {$"{RouteTablePrefix}/{transmitPrefix}/Remove", RemoveTransmitAddress},
                {$"{RouteTablePrefix}/{transmitPrefix}/GetAll", GetTransmitAll},
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
        /// 会话服务集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetSessionAll(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetAll();
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
        /// 获取包计数器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetAtomicCounter(JT808HttpRequest request)
        {
            JT808AtomicCounterDto jT808AtomicCounterDto = new JT808AtomicCounterDto();
            jT808AtomicCounterDto.MsgFailCount = jT808AtomicCounterService.MsgFailCount;
            jT808AtomicCounterDto.MsgSuccessCount = jT808AtomicCounterService.MsgSuccessCount;
            return CreateJT808HttpResponse(new JT808ResultDto<JT808AtomicCounterDto>
            {
                 Code=JT808ResultCode.Ok,
                 Data= jT808AtomicCounterDto
            });
        }

        /// <summary>
        /// 添加原包转发地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse AddSourcePackageAddress(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808IPAddressDto jT808IPAddressDto = JsonConvert.DeserializeObject<JT808IPAddressDto>(request.Json);
            return CreateJT808HttpResponse(jT808SourcePackageChannelService.Add(jT808IPAddressDto).Result);
        }

        /// <summary>
        /// 删除原包转发地址（不能删除在网关服务器配置文件配的地址）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveSourcePackageAddress(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808IPAddressDto jT808IPAddressDto = JsonConvert.DeserializeObject<JT808IPAddressDto>(request.Json);
            return CreateJT808HttpResponse(jT808SourcePackageChannelService.Remove(jT808IPAddressDto).Result);
        }

        /// <summary>
        /// 获取原包信息集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetSourcePackageAll(JT808HttpRequest request)
        {
            return CreateJT808HttpResponse(jT808SourcePackageChannelService.GetAll());
        }

        /// <summary>
        /// 添加转发过滤地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse AddTransmitAddress(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808IPAddressDto jT808IPAddressDto = JsonConvert.DeserializeObject<JT808IPAddressDto>(request.Json);
            return CreateJT808HttpResponse(jT808TransmitAddressFilterService.Add(jT808IPAddressDto));
        }

        /// <summary>
        /// 删除转发过滤地址（不能删除在网关服务器配置文件配的地址）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveTransmitAddress(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808IPAddressDto jT808IPAddressDto = JsonConvert.DeserializeObject<JT808IPAddressDto>(request.Json);
            return CreateJT808HttpResponse(jT808TransmitAddressFilterService.Remove(jT808IPAddressDto));
        }

        /// <summary>
        /// 获取转发过滤地址信息集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetTransmitAll(JT808HttpRequest request)
        {
            return CreateJT808HttpResponse(jT808TransmitAddressFilterService.GetAll());
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
                Code = JT808ResultCode.Empty,
                Message ="内容为空",
                 Data="Content Empty"
            }));
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse NotFoundHttpResponse()
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808ResultDto<string>()
            {
                 Code= JT808ResultCode.NotFound,
                 Message="没有该服务",
                 Data= "没有该服务"
            }));
            return new JT808HttpResponse(json);
        }

        public JT808HttpResponse ErrorHttpResponse(Exception ex)
        {
            byte[] json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new JT808ResultDto<string>()
            {
                Code = JT808ResultCode.Error,
                Message = JsonConvert.SerializeObject(ex),
                Data= ex.Message
            }));
            return new JT808HttpResponse(json);
        }
    }
}
