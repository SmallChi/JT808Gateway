using JT808.Gateway.Abstractions;
using JT808.Gateway.NormalHosting.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808SessionProducer : IJT808SessionProducer
    {
        public string TopicName { get; } = JT808GatewayConstants.SessionTopic;

        private readonly JT808SessionService JT808SessionService;

        public JT808SessionProducer(JT808SessionService jT808SessionService)
        {
            JT808SessionService = jT808SessionService;
        }

        public async void ProduceAsync(string notice,string terminalNo)
        {
            await JT808SessionService.WriteAsync(notice, terminalNo);
        }

        public void Dispose()
        {
        }
    }
}
