using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JT808.DotNetty.Abstractions
{
    public interface IJT808MsgConsumer : IJT808PubSub, IDisposable
    {
        void OnMessage(Action<(string TerminalNo, byte[] Data)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
