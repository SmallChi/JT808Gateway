using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace JT808.DotNetty.Dashbord.GrpcServer
{
    public class DemoServerHandler<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly ServerCallContext _context;

        public DemoServerHandler(ServerCallContext context)
        {
            _context = context;
        }

        public async Task<TResponse> UnaryServerHandler(TRequest request, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var response = await continuation(request, _context).ConfigureAwait(false);
            return response;
        }

        public async Task<TResponse> ClientStreamingServerHandler(IAsyncStreamReader<TRequest> requestStream, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            var response = await continuation(requestStream, _context).ConfigureAwait(false);
            return response;
        }

        public async Task ServerStreamingServerHandler(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await continuation(request, responseStream, _context).ConfigureAwait(false);
        }

        public async Task DuplexStreamingServerHandler(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await continuation(requestStream, responseStream, _context).ConfigureAwait(false);
        }
    }
}