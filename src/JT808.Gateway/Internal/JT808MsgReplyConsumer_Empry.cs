using System;
using System.Threading;
using JT808.Gateway.Abstractions;

namespace JT808.Gateway.Internal
{
    class JT808MsgReplyConsumer_Empry : IJT808MsgReplyConsumer
    {
        public CancellationTokenSource Cts { get; } = new CancellationTokenSource();

        public string TopicName { get; } = JT808GatewayConstants.MsgReplyTopic;

        public void Dispose()
        {

        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {

        }

        public void OnMessageWithDateTimeOffset(Action<(string TerminalNo, byte[] Data, DateTimeOffset dateTimeOffset)> callback)
        {

        }

        public void Subscribe()
        {

        }

        public void Unsubscribe()
        {

        }
    }
}
