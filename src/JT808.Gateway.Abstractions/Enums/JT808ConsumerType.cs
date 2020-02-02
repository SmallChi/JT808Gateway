using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions.Enums
{
    [Flags]
    public enum JT808ConsumerType:int
    {
        MsgIdHandlerConsumer=1,
        MsgLoggingConsumer=2,
        ReplyMessageConsumer=4,
        TrafficConsumer=8,
        TransmitConsumer=16,
        ReplyMessageLoggingConsumer = 32,
        All = 64,
    }
}
