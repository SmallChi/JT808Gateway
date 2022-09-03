using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Dtos;
using JT808.Gateway.Authorization;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JT808.Gateway
{
    /// <summary>
    /// jt808 webapi
    /// </summary>
    [ApiController]
    [Route("jt808api")]
    public sealed class JT808WebApi:ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        JT808SessionManager SessionManager;
        /// <summary>
        /// 
        /// </summary>
        JT808BlacklistManager BlacklistManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808SessionManager"></param>
        /// <param name="jT808BlacklistManager"></param>
        public JT808WebApi(
            JT808SessionManager jT808SessionManager,
            JT808BlacklistManager jT808BlacklistManager)
        {
            this.SessionManager = jT808SessionManager;
            this.BlacklistManager = jT808BlacklistManager;
        }

        /// <summary>
        /// index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("index")]    
        public ActionResult<JT808ResultDto<string>> Index()
        {
            JT808ResultDto<string> resultDto = new JT808ResultDto<string>();
            resultDto.Data = "Hello,JT808 WebApi";
            resultDto.Code = JT808ResultCode.Ok;
            return resultDto;
        }

        /// <summary>
        /// 统一下发设备消息服务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UnificationSend")]
        [JT808Token]
        public async Task<ActionResult<JT808ResultDto<bool>>> UnificationSend([FromBody] JT808UnificationSendRequestDto parameter)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                resultDto.Data = await SessionManager.TrySendByTerminalPhoneNoAsync(parameter.TerminalPhoneNo, Convert.FromHexString(parameter.HexData));
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-Tcp会话查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Tcp/Session/GetAll")]
        [JT808Token]
        public ActionResult<JT808ResultDto<List<JT808TcpSessionInfoDto>>> SessionTcpGetAll()
        {
            JT808ResultDto<List<JT808TcpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808TcpSessionInfoDto>>();
            try
            {
                resultDto.Data = SessionManager.GetTcpAll().Select(s => new JT808TcpSessionInfoDto
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
                resultDto.Data = new List<JT808TcpSessionInfoDto>();
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-Tcp分页会话查询
        /// jt808api/Tcp/Session/SessionTcpByPage?pageIndex=0&pageSize10
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Tcp/Session/SessionTcpByPage")]
        [JT808Token]
        public ActionResult<JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>> SessionTcpByPage([FromQuery]int pageIndex=0, [FromQuery] int pageSize=10)
        {
            JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>> resultDto = new JT808ResultDto<JT808PageResult<List<JT808TcpSessionInfoDto>>>();
            try
            {
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                if (pageSize >= 1000)
                {
                    pageSize = 1000;
                }
                JT808PageResult<List<JT808TcpSessionInfoDto>> pageResult = new JT808PageResult<List<JT808TcpSessionInfoDto>>();
                IEnumerable<JT808TcpSession> sessionInfoDtos = SessionManager.GetTcpByPage();
                pageResult.Data = sessionInfoDtos.Select(s => new JT808TcpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).OrderByDescending(o => o.LastActiveTime).Skip(pageIndex* pageSize).Take(pageSize).ToList();
                pageResult.Total = sessionInfoDtos.Count();
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                resultDto.Data = pageResult;
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = new JT808PageResult<List<JT808TcpSessionInfoDto>>();
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Tcp/Session/QuerySessionByTerminalPhoneNo")]
        [JT808Token]
        public ActionResult<JT808ResultDto<JT808TcpSessionInfoDto>> QueryTcpSessionByTerminalPhoneNo([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<JT808TcpSessionInfoDto> resultDto = new JT808ResultDto<JT808TcpSessionInfoDto>();
            try
            {
                resultDto.Data = SessionManager.GetTcpAll(w => w.TerminalPhoneNo == parameter.TerminalPhoneNo).Select(s => new JT808TcpSessionInfoDto
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
            return resultDto;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Tcp/Session/RemoveByTerminalPhoneNo")]
        [JT808Token]
        public ActionResult<JT808ResultDto<bool>> SessionTcpRemoveByTerminalPhoneNo([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                SessionManager.RemoveByTerminalPhoneNo(parameter.TerminalPhoneNo);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-Udp会话查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Udp/Session/GetAll")]
        [JT808Token]
        public ActionResult<JT808ResultDto<List<JT808UdpSessionInfoDto>>> SessionUdpGetAll()
        {
            JT808ResultDto<List<JT808UdpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808UdpSessionInfoDto>>();
            try
            {
                resultDto.Data = SessionManager.GetUdpAll().Select(s => new JT808UdpSessionInfoDto
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
                resultDto.Data = new List<JT808UdpSessionInfoDto>();
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-Udp分页会话查询
        /// jt808api/Udp/Session/SessionUdpByPage?pageIndex=0&pageSize10
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Udp/Session/SessionUdpByPage")]
        [JT808Token]
        public ActionResult<JT808ResultDto<JT808PageResult<List<JT808UdpSessionInfoDto>>>> SessionUdpByPage([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            JT808ResultDto<JT808PageResult<List<JT808UdpSessionInfoDto>>> resultDto = new JT808ResultDto<JT808PageResult<List<JT808UdpSessionInfoDto>>>();
            try
            {
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                if (pageSize >= 1000)
                {
                    pageSize = 1000;
                }
                JT808PageResult<List<JT808UdpSessionInfoDto>> pageResult = new JT808PageResult<List<JT808UdpSessionInfoDto>>();
                IEnumerable<JT808UdpSession> sessionInfoDtos = SessionManager.GetUdpByPage();
                pageResult.Data = sessionInfoDtos.Select(s => new JT808UdpSessionInfoDto
                {
                    LastActiveTime = s.ActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.RemoteEndPoint.ToString(),
                }).OrderByDescending(o => o.LastActiveTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                pageResult.Total = sessionInfoDtos.Count();
                pageResult.PageIndex = pageIndex;
                pageResult.PageSize = pageSize;
                resultDto.Data = pageResult;
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = new JT808PageResult<List<JT808UdpSessionInfoDto>>();
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 会话服务-通过设备终端号查询对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Udp/Session/QuerySessionByTerminalPhoneNo")]
        [JT808Token]
        public ActionResult<JT808ResultDto<JT808UdpSessionInfoDto>> QueryUdpSessionByTerminalPhoneNo([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<JT808UdpSessionInfoDto> resultDto = new JT808ResultDto<JT808UdpSessionInfoDto>();
            try
            {
                resultDto.Data = SessionManager.GetUdpAll(w => w.TerminalPhoneNo == parameter.TerminalPhoneNo).Select(s => new JT808UdpSessionInfoDto
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
            return resultDto;
        }

        /// <summary>
        /// 会话服务-通过设备终端号移除对应会话
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Udp/Session/RemoveByTerminalPhoneNo")]
        [JT808Token]
        public ActionResult<JT808ResultDto<bool>> SessionUdpRemoveByTerminalPhoneNo([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                SessionManager.RemoveByTerminalPhoneNo(parameter.TerminalPhoneNo);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 黑名单添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Blacklist/Add")]
        [JT808Token]
        public ActionResult<JT808ResultDto<bool>> BlacklistAdd([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                BlacklistManager.Add(parameter.TerminalPhoneNo);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 黑名单删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Blacklist/Remove")]
        [JT808Token]
        public ActionResult<JT808ResultDto<bool>> BlacklistRemove([FromBody] JT808TerminalPhoneNoDto parameter)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                BlacklistManager.Remove(parameter.TerminalPhoneNo);
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

        /// <summary>
        /// 黑名单查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Blacklist/GetAll")]
        [JT808Token]
        public ActionResult<JT808ResultDto<List<string>>> BlacklistGetAll()
        {
            JT808ResultDto<List<string>> resultDto = new JT808ResultDto<List<string>>();
            try
            {
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = BlacklistManager.GetAll();
            }
            catch (Exception ex)
            {
                resultDto.Data = new List<string>();
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.StackTrace;
            }
            return resultDto;
        }

    }
}
