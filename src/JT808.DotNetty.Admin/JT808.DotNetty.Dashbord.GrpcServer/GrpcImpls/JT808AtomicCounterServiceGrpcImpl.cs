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
            atomicCounterReply.AtomicCounterInfo = new AtomicCounterInfo
            {
                MsgFailCount = 10,
                MsgSuccessCount = 11111
            };
            atomicCounterReply.ResultReply = new ResultReply
            {
                Code = ResultReply.Types.StatusCode.Success,
            };
            return Task.FromResult(atomicCounterReply);
        }

        public override Task<AtomicCounterReply> GetUdpAtomicCounter(EmptyRequest request, ServerCallContext context)
        {
            AtomicCounterReply atomicCounterReply = new AtomicCounterReply();
            atomicCounterReply.AtomicCounterInfo = new AtomicCounterInfo
            {
                MsgFailCount = 50,
                MsgSuccessCount = 10000
            };
            atomicCounterReply.ResultReply = new ResultReply
            {
                Code = ResultReply.Types.StatusCode.Success,
            };
            return Task.FromResult(atomicCounterReply);
        }
    }
}
