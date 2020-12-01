using JT808.Gateway.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Internal
{
    class JT808MsgProducer_Empty : IJT808MsgProducer
    {
        public string TopicName { get; }

        public void Dispose()
        {

        }

        public void ProduceAsync(string terminalNo, byte[] data)
        {

        }
    }
}
