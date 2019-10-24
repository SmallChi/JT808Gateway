using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Configs.Kafka
{
    public class JT808MsgReplyProducerConfig : JT808ProducerConfig, IOptions<JT808MsgReplyProducerConfig>
    {
        JT808MsgReplyProducerConfig IOptions<JT808MsgReplyProducerConfig>.Value => this;
    }
}
