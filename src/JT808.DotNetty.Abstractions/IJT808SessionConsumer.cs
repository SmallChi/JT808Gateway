using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JT808.DotNetty.Abstractions
{
    /// <summary>
    /// 会话通知（在线/离线）
    /// </summary>
    public interface IJT808SessionConsumer : IJT808PubSub, IDisposable
    {
        void OnMessage(Action<(string Notice, string TerminalNo)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
