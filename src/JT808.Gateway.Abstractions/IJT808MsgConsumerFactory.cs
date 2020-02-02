using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808MsgConsumerFactory
    {
        IJT808MsgConsumer Create(JT808ConsumerType consumerType);
    }
}
