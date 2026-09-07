using System;
using System.Threading;

namespace JT808.Gateway.Abstractions
{
    /// <summary>
    /// 会话通知（在线/离线）
    /// </summary>
    public interface IJT808SessionConsumer : IJT808PubSub, IDisposable
    {
        void OnMessage(Action<(string Notice, string TerminalNo)> callback);
        void OnMessageWithDateTimeOffset(Action<(string Notice, string TerminalNo, DateTimeOffset dateTimeOffset)> callback);
        CancellationTokenSource Cts { get; }
        void Subscribe();
        void Unsubscribe();
    }
}
