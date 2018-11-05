using DotNetty.Buffers;
using JT808.DotNetty.Dtos;
using JT808.DotNetty.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationSendServiceDefaultImpl : IJT808UnificationSendService
    {
        private readonly JT808SessionManager jT808SessionManager;

        public JT808UnificationSendServiceDefaultImpl(JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                var session = jT808SessionManager.GetSessionByTerminalPhoneNo(terminalPhoneNo);
                session.Channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                resultDto.Data = true;
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = 500;
                resultDto.Message = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            }
            return resultDto;
        }
    }
}
