using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JT808.Gateway.InMemoryMQ.Services
{
    public class JT808ReplyMsgService
    {
        private readonly Channel<(string TerminalNo, byte[] Data)> _channel;

        public JT808ReplyMsgService()
        {
            _channel = Channel.CreateUnbounded<(string TerminalNo, byte[] Data)>();
        }

        public async ValueTask WriteAsync(string terminalNo, byte[] data)
        {
            await _channel.Writer.WriteAsync((terminalNo, data));
        }

        public bool TryRead(out (string TerminalNo, byte[] Data) item)
        {
            return _channel.Reader.TryRead(out item);
        }

        public async ValueTask<(string TerminalNo, byte[] Data)> ReadAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
