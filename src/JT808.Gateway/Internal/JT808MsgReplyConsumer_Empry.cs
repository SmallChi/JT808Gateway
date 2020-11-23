using JT808.Gateway.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        public void Subscribe()
        {

        }

        public void Unsubscribe()
        {

        }
    }
}
