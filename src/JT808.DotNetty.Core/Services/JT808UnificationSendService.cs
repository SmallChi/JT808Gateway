using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Session;
using System;
using System.Text.Json;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationSendService : IJT808UnificationSendService
    {
        private readonly JT808SessionManager jT808SessionManager;

        public JT808UnificationSendService(
            JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
        }

        public JT808ResultDto<bool> Send(string terminalPhoneNo, byte[] data)
        {
            JT808ResultDto<bool> resultDto = new JT808ResultDto<bool>();
            try
            {
                if(jT808SessionManager.TrySend(terminalPhoneNo, data, out var message))
                {
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = true;
                    resultDto.Message = message;
                }
                else
                {
                    resultDto.Code = JT808ResultCode.Ok;
                    resultDto.Data = false;
                    resultDto.Message = message;
                }
            }
            catch (Exception ex)
            {
                resultDto.Data = false;
                resultDto.Code = JT808ResultCode.Error;
                resultDto.Message = ex.Message;
            }
            return resultDto;
        }
    }
}
