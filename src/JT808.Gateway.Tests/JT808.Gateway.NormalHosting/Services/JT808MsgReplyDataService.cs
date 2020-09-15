using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Services
{
    public class JT808MsgReplyDataService
    {
        private readonly Channel<(string TerminalNo, byte[] Data)> _channel;

        public JT808MsgReplyDataService()
        {
            _channel = Channel.CreateUnbounded<(string TerminalNo, byte[] Data)>();
        }

        public async ValueTask WriteAsync(string terminalNo, byte[] Data)
        {
            await _channel.Writer.WriteAsync((terminalNo, Data));
        }
        public async ValueTask<(string TerminalNo, byte[] Data)> ReadAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
