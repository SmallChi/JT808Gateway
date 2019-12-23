using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808PubSub
    {
        string TopicName { get; }
    }
}
