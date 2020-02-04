using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Services
{
    public class JT808SessionService
    {
        private readonly Channel<(string Notice, string TerminalNo)> _channel;

        public JT808SessionService()
        {
            _channel = Channel.CreateUnbounded<(string Notice, string TerminalNo)>();
        }

        public async ValueTask WriteAsync(string notice, string terminalNo)
        {
            await _channel.Writer.WriteAsync((notice, terminalNo));
        }
        public async ValueTask<(string Notice, string TerminalNo)> ReadAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
