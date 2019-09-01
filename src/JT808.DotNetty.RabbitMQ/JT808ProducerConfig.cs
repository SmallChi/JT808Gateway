using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.RabbitMQ
{
    public class JT808ProducerConfig : IOptions<JT808ProducerConfig>
    {
        public string TopicName { get; set; }
        public string ConnectionString { get; set; }
        public JT808ProducerConfig Value => this;
    }
}
