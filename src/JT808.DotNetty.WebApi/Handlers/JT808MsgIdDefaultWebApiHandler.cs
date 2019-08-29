using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Metadata;
using JT808.DotNetty.Core.Services;
using Newtonsoft.Json;

namespace JT808.DotNetty.WebApi.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    public class JT808MsgIdDefaultWebApiHandler : JT808MsgIdHttpHandlerBase
    {
        private readonly JT808AtomicCounterService jT808TcpAtomicCounterService;

        private readonly JT808AtomicCounterService jT808UdpAtomicCounterService;

        private readonly IJT808TcpSessionService jT808TcpSessionService;

        private readonly IJT808UdpSessionService jT808UdpSessionService;

        private readonly IJT808UnificationTcpSendService jT808UnificationTcpSendService;

        private readonly IJT808UnificationUdpSendService jT808UnificationUdpSendService;

        /// <summary>
        /// TCP一套注入
        /// </summary>
        /// <param name="jT808TcpAtomicCounterService"></param>
        public JT808MsgIdDefaultWebApiHandler(
            IJT808UnificationTcpSendService jT808UnificationTcpSendService,
            IJT808TcpSessionService jT808TcpSessionService,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory
            )
        {
            this.jT808UnificationTcpSendService = jT808UnificationTcpSendService;
            this.jT808TcpSessionService = jT808TcpSessionService; 
            this.jT808TcpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            InitTcpRoute();
        }

        /// <summary>
        /// UDP一套注入
        /// </summary>
        /// <param name="jT808UdpAtomicCounterService"></param>
        public JT808MsgIdDefaultWebApiHandler(
            IJT808UdpSessionService jT808UdpSessionService,
            IJT808UnificationUdpSendService jT808UnificationUdpSendService,
            JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory
            )
        {
            this.jT808UdpSessionService = jT808UdpSessionService;
            this.jT808UnificationUdpSendService = jT808UnificationUdpSendService;
            this.jT808UdpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
            InitUdpRoute();
        }

        /// <summary>
        /// 统一的一套注入
        /// </summary>
        /// <param name="jT808TcpAtomicCounterService"></param>
        /// <param name="jT808UdpAtomicCounterService"></param>
        public JT808MsgIdDefaultWebApiHandler(
             IJT808UnificationTcpSendService jT808UnificationTcpSendService,
             IJT808UnificationUdpSendService jT808UnificationUdpSendService,
             IJT808TcpSessionService jT808TcpSessionService,
             IJT808UdpSessionService jT808UdpSessionService,
            JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory
           )
        {
            this.jT808UdpSessionService = jT808UdpSessionService;
            this.jT808UnificationTcpSendService = jT808UnificationTcpSendService;
            this.jT808UnificationUdpSendService = jT808UnificationUdpSendService;
            this.jT808TcpSessionService = jT808TcpSessionService;
            this.jT808TcpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.jT808UdpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
            InitTcpRoute();
            InitUdpRoute();
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetTcpSessionAll(JT808HttpRequest request)
        {
            var result = jT808TcpSessionService.GetAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveTcpSessionByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808TcpSessionService.RemoveByTerminalPhoneNo(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetUdpSessionAll(JT808HttpRequest request)
        {
            var result = jT808UdpSessionService.GetAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveUdpSessionByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808UdpSessionService.RemoveByTerminalPhoneNo(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 获取Tcp包计数器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetTcpAtomicCounter(JT808HttpRequest request)
        {
            JT808AtomicCounterDto jT808AtomicCounterDto = new JT808AtomicCounterDto();
            jT808AtomicCounterDto.MsgFailCount = jT808TcpAtomicCounterService.MsgFailCount;
            jT808AtomicCounterDto.MsgSuccessCount = jT808TcpAtomicCounterService.MsgSuccessCount;
            return CreateJT808HttpResponse(new JT808ResultDto<JT808AtomicCounterDto>
            {
                Code = JT808ResultCode.Ok,
                Data = jT808AtomicCounterDto
            });
        }

        /// <summary>
        /// 获取Udp包计数器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetUdpAtomicCounter(JT808HttpRequest request)
        {
            JT808AtomicCounterDto jT808AtomicCounterDto = new JT808AtomicCounterDto();
            jT808AtomicCounterDto.MsgFailCount = jT808UdpAtomicCounterService.MsgFailCount;
            jT808AtomicCounterDto.MsgSuccessCount = jT808UdpAtomicCounterService.MsgSuccessCount;
            return CreateJT808HttpResponse(new JT808ResultDto<JT808AtomicCounterDto>
            {
                Code = JT808ResultCode.Ok,
                Data = jT808AtomicCounterDto
            });
        }

        /// <summary>
        /// 基于Tcp的统一下发信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse UnificationTcpSend(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonConvert.DeserializeObject<JT808UnificationSendRequestDto>(request.Json);
            var result = jT808UnificationTcpSendService.Send(jT808UnificationSendRequestDto.TerminalPhoneNo, jT808UnificationSendRequestDto.Data);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 基于Udp的统一下发信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse UnificationUdpSend(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonConvert.DeserializeObject<JT808UnificationSendRequestDto>(request.Json);
            var result = jT808UnificationUdpSendService.Send(jT808UnificationSendRequestDto.TerminalPhoneNo, jT808UnificationSendRequestDto.Data);
            return CreateJT808HttpResponse(result);
        }

        protected virtual void InitTcpRoute()
        {
            CreateRoute(JT808Constants.JT808WebApiRouteTable.GetTcpAtomicCounter, GetTcpAtomicCounter);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.SessionTcpGetAll, GetTcpSessionAll);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.SessionTcpRemoveByTerminalPhoneNo, RemoveTcpSessionByTerminalPhoneNo);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.UnificationTcpSend, UnificationTcpSend);
        }

        protected virtual void InitUdpRoute()
        {
            CreateRoute(JT808Constants.JT808WebApiRouteTable.GetUdpAtomicCounter, GetUdpAtomicCounter);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.UnificationUdpSend, UnificationUdpSend);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.SessionUdpGetAll, GetUdpSessionAll);
            CreateRoute(JT808Constants.JT808WebApiRouteTable.SessionUdpRemoveByTerminalPhoneNo, RemoveUdpSessionByTerminalPhoneNo);
        }
    }
}
