using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dashbord.GrpcProtocol.Extensions
{
    public static class ErrorExtensions
    {
        public static void Unauthenticated(string msg= "Invalid Token")
        {
            throw new Grpc.Core.RpcException(new Status(StatusCode.Unauthenticated, msg));
        }
    }
}
