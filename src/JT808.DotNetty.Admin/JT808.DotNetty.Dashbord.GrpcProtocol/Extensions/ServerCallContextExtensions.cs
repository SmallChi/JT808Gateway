using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dashbord.GrpcProtocol.Extensions
{
    public static class ServerCallContextExtensions
    {
        public static void SetResultStatus(this ServerCallContext serverCallContext, StatusCode statusCode, string detail)
        {
            serverCallContext.Status = new Status(statusCode, detail);
        }

        public static void Ok(this ServerCallContext serverCallContext,string detail="")
        {
            serverCallContext.Status = new Status(StatusCode.OK, detail);
        }

        public static void Auth(this ServerCallContext serverCallContext, string detail = "")
        {
            serverCallContext.Status = new Status(StatusCode.Unauthenticated, detail);
        }

        public static void InternalError(this ServerCallContext serverCallContext, string detail = "")
        {
            serverCallContext.Status = new Status(StatusCode.Internal, detail);
        }
    }
}
