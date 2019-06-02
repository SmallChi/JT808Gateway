using DotNetty.Buffers;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using System;

namespace JT808.DotNetty.Internal
{
    internal class JT808UnificationUdpSendService : IJT808UnificationUdpSendService
    {
        private readonly JT808UdpSessionManager jT808SessionManager;

        private readonly IJT808DatagramPacket  jT808DatagramPacket;

        public JT808UnificationUdpSendService(
            IJT808DatagramPacket jT808DatagramPacket,
            JT808UdpSessionManager jT808SessionManager)
        {
            this.jT808DatagramPacket = jT808DatagramPacket;
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
                    session.Channel.WriteAndFlushAsync(jT808DatagramPacket.Create(data, session.Sender));
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
