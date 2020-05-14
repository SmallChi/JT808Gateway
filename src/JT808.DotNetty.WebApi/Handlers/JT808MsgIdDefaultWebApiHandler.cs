using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Metadata;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Internal;
using System.Text.Json;

namespace JT808.DotNetty.WebApi.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    public class JT808MsgIdDefaultWebApiHandler : JT808MsgIdHttpHandlerBase
    {
        private readonly JT808AtomicCounterService jT808TcpAtomicCounterService;

        private readonly JT808AtomicCounterService jT808UdpAtomicCounterService;

        private readonly IJT808SessionService jT808SessionService;

        private readonly IJT808UnificationSendService jT808UnificationSendService;

        public JT808MsgIdDefaultWebApiHandler(
            IJT808UnificationSendService jT808UnificationSendService,
            IJT808SessionService jT808SessionService,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory
            )
        {
            this.jT808UnificationSendService = jT808UnificationSendService;
            this.jT808SessionService = jT808SessionService;
            this.jT808TcpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.jT808UdpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
            InitTcpRoute();
            InitUdpRoute();
            InitCommontRoute();
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetTcpSessionAll(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetTcpAll();
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 通过终端手机号查询对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse QueryTcpSessionByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            return CreateJT808HttpResponse(jT808SessionService.GetTcpAll());
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse RemoveSessionByTerminalPhoneNo(JT808HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Json))
            {
                return EmptyHttpResponse();
            }
            var result = jT808SessionService.RemoveByTerminalPhoneNo(request.Json);
            return CreateJT808HttpResponse(result);
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JT808HttpResponse GetUdpSessionAll(JT808HttpRequest request)
        {
            var result = jT808SessionService.GetUdpAll();
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

            JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonSerializer.Deserialize<JT808UnificationSendRequestDto>(request.Json);
            var result = jT808UnificationSendService.Send(jT808UnificationSendRequestDto.TerminalPhoneNo, jT808UnificationSendRequestDto.Data);
            return CreateJT808HttpResponse(result);
        }

        protected virtual void InitCommontRoute()
        {
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.UnificationSend, UnificationSend);
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo, RemoveSessionByTerminalPhoneNo);
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.QueryTcpSessionByTerminalPhoneNo, QueryTcpSessionByTerminalPhoneNo);
        }

        protected virtual void InitTcpRoute()
        {
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.GetTcpAtomicCounter, GetTcpAtomicCounter);
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.SessionTcpGetAll, GetTcpSessionAll);
        }

        protected virtual void InitUdpRoute()
        {
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.GetUdpAtomicCounter, GetUdpAtomicCounter);
            CreateRoute(JT808NettyConstants.JT808WebApiRouteTable.SessionUdpGetAll, GetUdpSessionAll);
        }
    }
}
