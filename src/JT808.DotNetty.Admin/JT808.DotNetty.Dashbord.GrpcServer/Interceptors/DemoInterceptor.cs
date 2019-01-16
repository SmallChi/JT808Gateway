using Grpc.Core;
using Grpc.Core.Interceptors;
using JT808.DotNetty.Dashbord.GrpcProtocol.Extensions;
using JT808.GrpcDashbord.ServiceGrpcBase;
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
    /// https://github.com/grpc/proposal/blob/master/L12-csharp-interceptors.md
    /// https://stackoverflow.com/questions/52950210/populate-authcontext-in-grpc-c-sharp-from-jwt-authentication
    /// https://github.com/grpc/grpc/tree/master/doc
    /// </summary>
    class DemoInterceptor : Interceptor
    {
        public override  Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            if(TryGetValue(context.RequestHeaders,"token",out var str))
            {
                //ErrorExtensions.Unauthenticated();
            }
            return continuation(request, context);
            //return Task.FromResult(default(TResponse));
            //return await continuation(request, context);
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
