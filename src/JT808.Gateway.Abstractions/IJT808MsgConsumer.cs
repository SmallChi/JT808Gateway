using System;
using System.Threading;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808MsgConsumer : IJT808PubSub, IDisposable
    {
        void OnMessage(Action<(string TerminalNo, byte[] Data)> callback);
        void OnMessageWithDateTimeOffset(Action<(string TerminalNo, byte[] Data, DateTimeOffset dateTimeOffset)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
