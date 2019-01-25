using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using JT808.GrpcDashbord.AtomicCounterGrpcService;
using JT808.GrpcDashbord.ServiceGrpcBase;
using static JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterService;

namespace JT808.DotNetty.Dashbord.GrpcServer.GrpcImpls
{
    public class JT808AtomicCounterServiceGrpcImpl: AtomicCounterServiceBase
    {
        public override Task<AtomicCounterReply> GetTcpAtomicCounter(EmptyRequest request, ServerCallContext context)
        {
            AtomicCounterReply atomicCounterReply = new AtomicCounterReply();
            atomicCounterReply.MsgFailCount = 10;
            atomicCounterReply.MsgSuccessCount = 1111;
            return Task.FromResult(atomicCounterReply);
        }

        public override Task<AtomicCounterReply> GetUdpAtomicCounter(EmptyRequest request, ServerCallContext context)
        {
            AtomicCounterReply atomicCounterReply = new AtomicCounterReply();
            atomicCounterReply.MsgFailCount = 50;
            atomicCounterReply.MsgSuccessCount = 1111;
            return Task.FromResult(atomicCounterReply);
        }
    }
}
