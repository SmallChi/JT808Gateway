using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Session;

namespace JT808.DotNetty.Core.Services
{
    internal class JT808SessionService : IJT808SessionService
    {
        private readonly JT808SessionManager jT808SessionManager;

        public JT808SessionService(
            JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<List<JT808TcpSessionInfoDto>> GetTcpAll()
        {
            JT808ResultDto<List<JT808TcpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808TcpSessionInfoDto>>();
            try
            {
                resultDto.Data = jT808SessionManager.GetAll().Select(s => new JT808TcpSessionInfoDto
                {
                    LastActiveTime = s.LastActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.Channel.RemoteAddress.ToString(),
                }).ToList();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.Message;
            }
            return resultDto;
        }

        public JT808ResultDto<List<JT808UdpSessionInfoDto>> GetUdpAll()
        {
            JT808ResultDto<List<JT808UdpSessionInfoDto>> resultDto = new JT808ResultDto<List<JT808UdpSessionInfoDto>>();
            try
            {
                resultDto.Data = jT808SessionManager.GetUdpAll().Select(s => new JT808UdpSessionInfoDto
                {
                    LastActiveTime = s.LastActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.Sender.ToString()
                }).ToList();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.Message;
            }
            return resultDto;
        }

        public JT808ResultDto<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.RemoveSession(terminalPhoneNo);
                if (session != null)
                {
                    if(session.Channel.Open)
                    {
                        session.Channel.CloseAsync();
                    }
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = true;
                }
                else
                {
                    resultDto.Code = JT808ResultCode.Empty;
                    resultDto.Data = false;
                    resultDto.Message = "Session Empty";
                }
            }
            catch (AggregateException ex)
            {
                resultDto.Data = false;
                resultDto.Code = 500;
                resultDto.Message = ex.Message;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.Message;
            }
            return resultDto;
        }

        public JT808ResultDto<JT808TcpSessionInfoDto> QueryTcpSessionByTerminalPhoneNo(string terminalPhoneNo)
        {
            JT808ResultDto<JT808TcpSessionInfoDto> resultDto = new JT808ResultDto<JT808TcpSessionInfoDto>();
            try
            {
                resultDto.Data = jT808SessionManager.GetAll().Where(w=>w.TerminalPhoneNo== terminalPhoneNo).Select(s => new JT808TcpSessionInfoDto
                {
                    LastActiveTime = s.LastActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo,
                    RemoteAddressIP = s.Channel.RemoteAddress.ToString(),
                }).FirstOrDefault();
                resultDto.Code = JT808ResultCode.Ok;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.Message;
            }
            return resultDto;
        }
    }
}
