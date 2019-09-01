using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Kafka
{
    public class JT808ConsumerConfig: ConsumerConfig, IOptions<JT808ConsumerConfig>
    {
        public string TopicName { get; set; }

        public JT808ConsumerConfig Value => this;
    }
}
