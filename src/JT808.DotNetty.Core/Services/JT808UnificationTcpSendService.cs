using DotNetty.Buffers;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using System;
using System.Linq;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationTcpSendService : IJT808UnificationTcpSendService
    {
        private readonly JT808TcpSessionManager jT808SessionManager;

        private readonly JT808TcpTrafficService  jT808TcpTrafficService;

        public JT808UnificationTcpSendService(
            JT808TcpTrafficService jT808TcpTrafficService,
            JT808TcpSessionManager jT808SessionManager)
        {
            this.jT808TcpTrafficService = jT808TcpTrafficService;
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.GetSession(terminalPhoneNo);
                if (session != null)
                {
                    //判断转发数据是下发不了消息的
                    if (jT808SessionManager.GetAll().Count(c => c.Channel.Id == session.Channel.Id) > 1)
                    {
                        resultDto.Code = JT808ResultCode.Ok;
                        resultDto.Data = false;
                        resultDto.Message = "not support transmit data send.";
                    }
                    else
                    {
                        jT808TcpTrafficService.SendSize(data.Length);
                        session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                        resultDto.Code = JT808ResultCode.Ok;
                        resultDto.Data = true;
                    }
                }
                else
                {
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = false;
                    resultDto.Message = "offline";
                }
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
