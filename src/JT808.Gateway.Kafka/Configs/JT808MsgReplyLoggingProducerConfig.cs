using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Configs.Kafka
{
    public class JT808MsgReplyLoggingProducerConfig : JT808ProducerConfig, IOptions<JT808MsgReplyLoggingProducerConfig>
    {
        JT808MsgReplyLoggingProducerConfig IOptions<JT808MsgReplyLoggingProducerConfig>.Value => this;
    }
}
