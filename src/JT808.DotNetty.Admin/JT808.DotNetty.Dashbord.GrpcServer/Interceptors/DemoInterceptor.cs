using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Dashbord.GrpcServer.Interceptors
{
    /// <summary>
    /// 
    /// https://github.com/grpc/grpc/blob/master/doc/server_side_auth.md
    /// https://github.com/Falco20019/grpc-opentracing
    /// </summary>
    class DemoInterceptor : Interceptor
    {
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            if(TryGetValue(context.RequestHeaders,"token",out var str))
            {
                //context.Status = new Status(StatusCode.Unauthenticated, "Invalid token");
                return default(Task<TResponse>);
            }
            return continuation(request, context);
        }

        private bool TryGetValue(Metadata metadata,string key,out string value)
        {
            foreach(var item in metadata)
            {
                if(item.Key== key)
                {
                    value = item.Value;
                    return true;
                }
            }
            value = "";
            return false;
        }
    }
}
