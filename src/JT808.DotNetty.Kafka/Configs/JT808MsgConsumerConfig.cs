using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Kafka
{
    public class JT808MsgConsumerConfig : JT808ConsumerConfig, IOptions<JT808MsgConsumerConfig>
    {
        JT808MsgConsumerConfig IOptions<JT808MsgConsumerConfig>.Value => this;
    }
}
