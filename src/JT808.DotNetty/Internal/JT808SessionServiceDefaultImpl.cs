using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using JT808.DotNetty.Configurations;

namespace JT808.DotNetty.Internal
{
    internal class JT808SessionServiceDefaultImpl : IJT808SessionService
    {
        private readonly JT808SessionManager jT808SessionManager;

        public JT808SessionServiceDefaultImpl(
            JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<List<JT808SessionInfoDto>> GetAll()
        {
            JT808ResultDto<List<JT808SessionInfoDto>> resultDto = new JT808ResultDto<List<JT808SessionInfoDto>>();
            try
            {
                resultDto.Data = jT808SessionManager.GetAll().Select(s => new JT808SessionInfoDto
                {
                    ChannelId = s.SessionID,
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
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<bool> RemoveByChannelId(string channelId)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.RemoveSessionByID(channelId);
                if (session != null)
                {
                    session.Channel.CloseAsync();
                }
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (AggregateException ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.RemoveSessionByTerminalPhoneNo(terminalPhoneNo);
                if (session != null)
                {
                    session.Channel.CloseAsync();
                }
                resultDto.Code = JT808ResultCode.Ok;
                resultDto.Data = true;
            }
            catch (AggregateException ex)
            {
                resultDto.Data = false;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }
    }
}
