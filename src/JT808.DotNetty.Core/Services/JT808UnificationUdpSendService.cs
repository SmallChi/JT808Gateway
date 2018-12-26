using DotNetty.Buffers;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using System;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationUdpSendService : IJT808UnificationUdpSendService
    {
        private readonly JT808UdpSessionManager jT808SessionManager;

        public JT808UnificationUdpSendService(JT808UdpSessionManager jT808SessionManager)
        {
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
                    if (session.Channel.Open)
                    {
                        session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                        resultDto.Code = JT808ResultCode.Ok;
                        resultDto.Data = true;
                    }
                    else
                    {
                        resultDto.Code = JT808ResultCode.Ok;
                        resultDto.Data = false;
                        resultDto.Message = "offline";
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
