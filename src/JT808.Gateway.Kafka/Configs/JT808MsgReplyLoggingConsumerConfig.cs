using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Configs.Kafka
{
    public class JT808MsgReplyLoggingConsumerConfig : JT808ConsumerConfig, IOptions<JT808MsgReplyLoggingConsumerConfig>
    {
        JT808MsgReplyLoggingConsumerConfig IOptions<JT808MsgReplyLoggingConsumerConfig>.Value => this;
    }
}
