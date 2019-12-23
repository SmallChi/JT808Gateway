using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Configs.Kafka
{
    public class JT808ProducerConfig : ProducerConfig,IOptions<JT808ProducerConfig>
    {
        public string TopicName { get; set; }

        public JT808ProducerConfig Value => this;
    }
}
