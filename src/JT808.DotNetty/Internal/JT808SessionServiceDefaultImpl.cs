using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace JT808.DotNetty.Internal
{
    internal class JT808SessionServiceDefaultImpl : IJT808SessionService
    {
        private readonly JT808SessionManager jT808SessionManager;

        public JT808SessionServiceDefaultImpl(JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<JT808SessionInfoDto> GetByChannelId(string channelId)
        {
            JT808ResultDto<JT808SessionInfoDto> resultDto = new JT808ResultDto<JT808SessionInfoDto>();
            try
            {
                var result = jT808SessionManager.GetSessionByID(channelId);
                JT808SessionInfoDto jT808SessionInfoDto = new JT808SessionInfoDto
                {
                    TerminalPhoneNo = result.TerminalPhoneNo,
                    ChannelId=result.SessionID,
                    LastActiveTime=result.LastActiveTime,
                    StartTime=result.StartTime
                };
                resultDto.Code = 200;
                resultDto.Data = jT808SessionInfoDto;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<int> GetRelevanceLinkCount()
        {
            JT808ResultDto<int> resultDto = new JT808ResultDto<int>();
            try
            {
                resultDto.Data = jT808SessionManager.RelevanceSessionCount;
                resultDto.Code = 200;
            }
            catch (Exception ex)
            {
                resultDto.Data = 0;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<int> GetRealLinkCount()
        {
            JT808ResultDto<int> resultDto = new JT808ResultDto<int>();
            try
            {
                resultDto.Data = jT808SessionManager.RealSessionCount;
                resultDto.Code = 200;
            }
            catch (Exception ex)
            {
                resultDto.Data = 0;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<List<JT808SessionInfoDto>> GetRealAll()
        {
            JT808ResultDto<List<JT808SessionInfoDto>> resultDto = new JT808ResultDto<List<JT808SessionInfoDto>>();
            try
            {
                resultDto.Data = jT808SessionManager.GetRealAll().Select(s => new JT808SessionInfoDto
                {
                    ChannelId = s.SessionID,
                    LastActiveTime = s.LastActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo
                }).ToList();
                resultDto.Code = 200;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<List<JT808SessionInfoDto>> GetRelevanceAll()
        {
            JT808ResultDto<List<JT808SessionInfoDto>> resultDto = new JT808ResultDto<List<JT808SessionInfoDto>>();
            try
            {
                resultDto.Data = jT808SessionManager.GetRelevanceAll().Select(s => new JT808SessionInfoDto
                {
                    ChannelId = s.SessionID,
                    LastActiveTime = s.LastActiveTime,
                    StartTime = s.StartTime,
                    TerminalPhoneNo = s.TerminalPhoneNo
                }).ToList();
                resultDto.Code = 200;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = 500;
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
                resultDto.Code = 200;
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
                resultDto.Code = 500;
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
                resultDto.Code = 200;
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
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }

        public JT808ResultDto<JT808SessionInfoDto> GetByTerminalPhoneNo(string terminalPhoneNo)
        {
            JT808ResultDto<JT808SessionInfoDto> resultDto = new JT808ResultDto<JT808SessionInfoDto>();
            try
            {
                var result = jT808SessionManager.GetSessionByTerminalPhoneNo(terminalPhoneNo);
                JT808SessionInfoDto jT808SessionInfoDto = new JT808SessionInfoDto
                {
                    TerminalPhoneNo = result.TerminalPhoneNo,
                    ChannelId = result.SessionID,
                    LastActiveTime = result.LastActiveTime,
                    StartTime = result.StartTime
                };
                resultDto.Code = 200;
                resultDto.Data = jT808SessionInfoDto;
            }
            catch (Exception ex)
            {
                resultDto.Data = null;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }
    }
}
