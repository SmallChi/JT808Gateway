using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Dtos;
using JT808.Gateway.Session;
using JT808.Protocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JT808.Gateway.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    public class JT808MsgIdDefaultWebApiHandler : JT808MsgIdHttpHandlerBase
    {
        private  JT808SessionManager JT808SessionManager;
        public JT808MsgIdDefaultWebApiHandler(JT808SessionManager jT808SessionManager)
        {
            this.JT808SessionManager = jT808SessionManager;
            InitTcpRoute();
            InitUdpRoute();
            InitCommontRoute();
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] GetTcpSessionAll(string json)
        {
            JT808ResultDto<List<JT808TcpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808TcpSessionInfoDto>>();
            try
            {
                resultDto.Data = JT808SessionManager.GetTcpAll().Select(s => new JT808TcpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).ToList();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 通过终端手机号查询对应会话
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] QueryTcpSessionByTerminalPhoneNo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return EmptyHttpResponse();
            }
            JT808ResultDto<JT808TcpSessionInfoDto> resultDto = new JT808ResultDto<JT808TcpSessionInfoDto>();
            try
            {
                resultDto.Data = JT808SessionManager.GetTcpAll().Where(w => w.TerminalPhoneNo == json).Select(s => new JT808TcpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).FirstOrDefault();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] RemoveSessionByTerminalPhoneNo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return EmptyHttpResponse();
            }
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                JT808SessionManager.RemoveByTerminalPhoneNo(json);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (AggregateException ex)
            {
                resultDto.Data = false;
                resultDto.Code = 500;
                resultDto.Message = ex.StackTrace;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 会话服务集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] GetUdpSessionAll(string json)
        {
            JT808ResultDto<List<JT808UdpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808UdpSessionInfoDto>>();
            try
            {
                resultDto.Data = JT808SessionManager.GetUdpAll().Select(s => new JT808UdpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).ToList();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 通过终端手机号查询对应会话
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] QueryUdpSessionByTerminalPhoneNo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return EmptyHttpResponse();
            }
            JT808ResultDto<JT808UdpSessionInfoDto> resultDto = new JT808ResultDto<JT808UdpSessionInfoDto>();
            try
            {
                resultDto.Data = JT808SessionManager.GetUdpAll().Where(w => w.TerminalPhoneNo == json).Select(s => new JT808UdpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).FirstOrDefault();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] RemoveUdpByTerminalPhoneNo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return EmptyHttpResponse();
            }
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                JT808SessionManager.RemoveByTerminalPhoneNo(json);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (AggregateException ex)
            {
                resultDto.Data = false;
                resultDto.Code = 500;
                resultDto.Message = ex.StackTrace;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        /// <summary>
        /// 统一下发信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public byte[] UnificationSend(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return EmptyHttpResponse();
            }
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                JT808UnificationSendRequestDto jT808UnificationSendRequestDto = JsonSerializer.Deserialize<JT808UnificationSendRequestDto>(json);
                resultDto.Data = JT808SessionManager.TrySendByTerminalPhoneNoAsync(jT808UnificationSendRequestDto.TerminalPhoneNo, jT808UnificationSendRequestDto.HexData.ToHexBytes())
                                                .GetAwaiter()
                                                .GetResult();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return CreateHttpResponse(resultDto);
        }

        protected virtual void InitCommontRoute()
        {
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.UnificationSend, UnificationSend);
        }

        protected virtual void InitTcpRoute()
        {
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.SessionTcpGetAll, GetTcpSessionAll);
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.QueryTcpSessionByTerminalPhoneNo, QueryTcpSessionByTerminalPhoneNo);
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.SessionRemoveByTerminalPhoneNo, RemoveSessionByTerminalPhoneNo);
        }

        protected virtual void InitUdpRoute()
        {
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.SessionUdpGetAll, GetUdpSessionAll);
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.QueryUdpSessionByTerminalPhoneNo, QueryUdpSessionByTerminalPhoneNo);
            CreateRoute(JT808GatewayConstants.JT808WebApiRouteTable.RemoveUdpByTerminalPhoneNo, RemoveUdpByTerminalPhoneNo);
        }
    }
}
