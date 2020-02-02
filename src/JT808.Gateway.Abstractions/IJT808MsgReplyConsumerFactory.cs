using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808MsgReplyConsumerFactory
    {
        IJT808MsgReplyConsumer Create(JT808ConsumerType consumerType);
    }
}
