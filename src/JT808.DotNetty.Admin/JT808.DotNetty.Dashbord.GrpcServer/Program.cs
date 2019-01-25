using Grpc.Core;
using Grpc.Core.Interceptors;
using JT808.DotNetty.Dashbord.GrpcServer.GrpcImpls;
using JT808.DotNetty.Dashbord.GrpcServer.Interceptors;
using JT808.GrpcDashbord.AtomicCounterGrpcService;
using System;
using System.Threading;
using static JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterService;

namespace JT808.DotNetty.Dashbord.GrpcServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server
            {
                Services = {
                    BindService(new JT808AtomicCounterServiceGrpcImpl())
                    .Intercept(new DemoInterceptor()),
                },
                Ports = {
                    new ServerPort("0.0.0.0", 14000,ServerCredentials.Insecure)
                }
            };
            Console.WriteLine("Google Grpc Starting");
            foreach (var item in server.Ports)
            {
                Console.WriteLine(string.Format("RPC server {0} listening on port {1}", item.Host, item.Port));
            }
            server.Start();
            AtomicCounterServiceClient client = new AtomicCounterServiceClient(new Channel("127.0.0.1:14000", ChannelCredentials.Insecure));
            Metadata metadata = new Metadata();
            metadata.Add("token", "test");
            metadata.Add("request", "web");
           
            try
            {
                var result = client.GetTcpAtomicCounter(new GrpcDashbord.ServiceGrpcBase.EmptyRequest(), metadata);
            }
            catch (RpcException ex)
            {

            }
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }
}
