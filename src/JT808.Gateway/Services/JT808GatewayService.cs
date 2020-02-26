using System;
using System.Linq;
using JT808.Gateway.GrpcService;
using Grpc.Core;
using System.Threading.Tasks;
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.Session;
using Microsoft.Extensions.DependencyInjection;
using static Grpc.Core.Metadata;
using Microsoft.Extensions.Options;
using JT808.Gateway.Configurations;

namespace JT808.Gateway.Services
{
    public class JT808GatewayService: JT808Gateway.JT808GatewayBase
    {
        private readonly JT808AtomicCounterService jT808TcpAtomicCounterService;

        private readonly JT808AtomicCounterService jT808UdpAtomicCounterService;

        private readonly JT808SessionManager jT808SessionManager;

        private readonly IOptionsMonitor<JT808Configuration> ConfigurationOptionsMonitor;

        public JT808GatewayService(
            IOptionsMonitor<JT808Configuration> configurationOptionsMonitor,
            JT808SessionManager jT808SessionManager,
            JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory
            )
        {
            this.jT808SessionManager = jT808SessionManager;
            this.ConfigurationOptionsMonitor = configurationOptionsMonitor;
            this.jT808TcpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.jT808UdpAtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
        }

        public JT808GatewayService(IServiceProvider serviceProvider)
        {
            this.jT808SessionManager = serviceProvider.GetRequiredService<JT808SessionManager>();
            this.jT808TcpAtomicCounterService = serviceProvider.GetRequiredService<JT808AtomicCounterServiceFactory>().Create(JT808TransportProtocolType.tcp);
            this.jT808UdpAtomicCounterService = serviceProvider.GetRequiredService<JT808AtomicCounterServiceFactory>().Create(JT808TransportProtocolType.udp);
            this.ConfigurationOptionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<JT808Configuration>>();
        }

        public override Task<TcpSessionInfoReply> GetTcpSessionAll(Empty request, ServerCallContext context)
        {
            Auth(context);
            var result = jT808SessionManager.GetTcpAll();
            TcpSessionInfoReply reply = new TcpSessionInfoReply();
            foreach (var item in result)
            {
                reply.TcpSessions.Add(new SessionInfo
                {
                    LastActiveTime = item.ActiveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    RemoteAddressIP = item.RemoteEndPoint.ToString(),
                    TerminalPhoneNo = item.TerminalPhoneNo
                });
            }     
            return Task.FromResult(reply);
        }

        public override Task<SessionInfo> GetTcpSessionByTerminalPhoneNo(SessionRequest request, ServerCallContext context)
        {
            Auth(context);
            var result = jT808SessionManager.GetTcpAll().FirstOrDefault(f=>f.TerminalPhoneNo==request.TerminalPhoneNo);
            SessionInfo sessionInfo = new SessionInfo();
            if (result != null)
            {
                sessionInfo.LastActiveTime = result.ActiveTime.ToString("yyyy-MM-dd HH:mm:ss");
                sessionInfo.StartTime = result.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                sessionInfo.RemoteAddressIP = result.RemoteEndPoint.ToString();
                sessionInfo.TerminalPhoneNo = result.TerminalPhoneNo;
                return Task.FromResult(sessionInfo);
            }
            else
            {
                throw new Grpc.Core.RpcException(new Status(StatusCode.FailedPrecondition, $"{request.TerminalPhoneNo} not exists"));
            }
        }

        public override Task<SessionCountReply> GetTcpSessionCount(Empty request, ServerCallContext context)
        {
            Auth(context);
            return Task.FromResult(new SessionCountReply
            {
                Count = jT808SessionManager.TcpSessionCount
            });
        }

        public override Task<SessionRemoveReply> RemoveSessionByTerminalPhoneNo(SessionRemoveRequest request, ServerCallContext context)
        {
            Auth(context);
            try
            {
                jT808SessionManager.RemoveByTerminalPhoneNo(request.TerminalPhoneNo);
                return Task.FromResult(new SessionRemoveReply { Success = true });
            }
            catch (Exception)
            {
                return Task.FromResult(new SessionRemoveReply { Success = false });
            }
        }

        public override Task<UdpSessionInfoReply> GetUdpSessionAll(Empty request, ServerCallContext context)
        {
            Auth(context);
            var result = jT808SessionManager.GetUdpAll();
            UdpSessionInfoReply reply = new UdpSessionInfoReply();
            foreach (var item in result)
            {
                reply.UdpSessions.Add(new SessionInfo
                {
                    LastActiveTime = item.ActiveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    RemoteAddressIP = item.RemoteEndPoint.ToString(),
                    TerminalPhoneNo = item.TerminalPhoneNo
                });
            }
          
            return Task.FromResult(reply);
        }

        public override async Task<UnificationSendReply> UnificationSend(UnificationSendRequest request, ServerCallContext context)
        {
            Auth(context);
            try
            {
                var flag = await jT808SessionManager.TrySendByTerminalPhoneNoAsync(request.TerminalPhoneNo, request.Data.ToByteArray());
                return  new UnificationSendReply { Success = flag };
            }
            catch (Exception)
            {
                return  new UnificationSendReply { Success = false };
            }
        }

        public override Task<TcpAtomicCounterReply> GetTcpAtomicCounter(Empty request, ServerCallContext context)
        {
            Auth(context);
            TcpAtomicCounterReply reply = new TcpAtomicCounterReply();
            reply.MsgFailCount=jT808TcpAtomicCounterService.MsgFailCount;
            reply.MsgSuccessCount=jT808TcpAtomicCounterService.MsgSuccessCount;
            return Task.FromResult(reply);
        }

        public override Task<UdpAtomicCounterReply> GetUdpAtomicCounter(Empty request, ServerCallContext context)
        {
            Auth(context);
            UdpAtomicCounterReply reply = new UdpAtomicCounterReply();
            reply.MsgFailCount = jT808UdpAtomicCounterService.MsgFailCount;
            reply.MsgSuccessCount = jT808UdpAtomicCounterService.MsgSuccessCount;
            return Task.FromResult(reply);
        }

        private void Auth(ServerCallContext context)
        {
            Entry tokenEntry = context.RequestHeaders.FirstOrDefault(w => w.Key == "token");
            if (tokenEntry != null)
            {
                if(tokenEntry.Value != ConfigurationOptionsMonitor.CurrentValue.WebApiToken)
                {
                    throw new Grpc.Core.RpcException(new Status(StatusCode.Unauthenticated, "token error"));
                }
            }
            else
            {
                throw new Grpc.Core.RpcException(new Status(StatusCode.Unauthenticated,"token empty"));
            }
        }
    }
}
