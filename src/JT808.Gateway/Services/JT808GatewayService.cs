using JT808.Gateway.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Gateway.Enums;
using JT808.Gateway.GrpcService;
using Grpc.Core;
using System.Threading.Tasks;

namespace JT808.Gateway.Services
{
    public class JT808GatewayService: JT808Gateway.JT808GatewayBase
    {
        private readonly JT808AtomicCounterService jT808TcpAtomicCounterService;

        private readonly JT808AtomicCounterService jT808UdpAtomicCounterService;

        private readonly IJT808SessionService jT808SessionService;

        private readonly IJT808UnificationSendService jT808UnificationSendService;

        public JT808GatewayService(
            IJT808UnificationSendService jT808UnificationSendService,
            IJT808SessionService jT808SessionService,
            JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory
            )
        {
            this.jT808UnificationSendService = jT808UnificationSendService;
            this.jT808SessionService = jT808SessionService;
            this.jT808TcpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.jT808UdpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
        }

        public override Task<TcpSessionInfoReply> GetTcpSessionAll(Empty request, ServerCallContext context)
        {
            var result = jT808SessionService.GetTcpAll();
            TcpSessionInfoReply reply = new TcpSessionInfoReply();
            foreach(var item in result.Data)
            {
                reply.TcpSessions.Add(new SessionInfo
                {
                     LastActiveTime= item.LastActiveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                     StartTime= item.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                     RemoteAddressIP= item.RemoteAddressIP,
                     TerminalPhoneNo=item.TerminalPhoneNo
                });
            }
            return Task.FromResult(reply);
        }

        public override Task<SessionRemoveReply> RemoveSessionByTerminalPhoneNo(SessionRemoveRequest request, ServerCallContext context)
        {
            var result = jT808SessionService.RemoveByTerminalPhoneNo(request.TerminalPhoneNo);
            return Task.FromResult(new SessionRemoveReply { Success = result.Data });
        }

        public override Task<UdpSessionInfoReply> GetUdpSessionAll(Empty request, ServerCallContext context)
        {
            var result = jT808SessionService.GetUdpAll();
            UdpSessionInfoReply reply = new UdpSessionInfoReply();
            foreach (var item in result.Data)
            {
                reply.UdpSessions.Add(new SessionInfo
                {
                    LastActiveTime = item.LastActiveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    RemoteAddressIP = item.RemoteAddressIP,
                    TerminalPhoneNo = item.TerminalPhoneNo
                });
            }
            return Task.FromResult(reply);
        }

        public override Task<UnificationSendReply> UnificationSend(UnificationSendRequest request, ServerCallContext context)
        {
            var result = jT808UnificationSendService.Send(request.TerminalPhoneNo, request.Data.ToByteArray());
            return Task.FromResult(new UnificationSendReply { Success = result.Data });
        }

        public override Task<TcpAtomicCounterReply> GetTcpAtomicCounter(Empty request, ServerCallContext context)
        {
            TcpAtomicCounterReply reply = new TcpAtomicCounterReply();
            reply.MsgFailCount=jT808TcpAtomicCounterService.MsgFailCount;
            reply.MsgSuccessCount=jT808TcpAtomicCounterService.MsgSuccessCount;
            return Task.FromResult(reply);
        }

        public override Task<UdpAtomicCounterReply> GetUdpAtomicCounter(Empty request, ServerCallContext context)
        {
            UdpAtomicCounterReply reply = new UdpAtomicCounterReply();
            reply.MsgFailCount = jT808UdpAtomicCounterService.MsgFailCount;
            reply.MsgSuccessCount = jT808UdpAtomicCounterService.MsgSuccessCount;
            return Task.FromResult(reply);
        }
    }
}
