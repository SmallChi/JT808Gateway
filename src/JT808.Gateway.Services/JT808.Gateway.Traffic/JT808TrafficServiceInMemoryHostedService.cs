using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using JT808.Protocol.Extensions;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using System;

namespace JT808.Gateway.Traffic
{
    public class JT808TrafficServiceInMemoryHostedService : IHostedService
    {
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        private readonly IJT808Traffic  jT808Traffic;

        public JT808TrafficServiceInMemoryHostedService(
            IJT808Traffic jT808Traffic,
            IJT808MsgConsumerFactory  jT808MsgConsumerFactory)
        {
            this.jT808MsgConsumer = jT808MsgConsumerFactory.Create(JT808ConsumerType.TrafficConsumer);
            this.jT808Traffic = jT808Traffic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage((item)=> {
                //string str = item.Data.ToHexString();
                jT808Traffic.Increment(item.TerminalNo, DateTime.Now.ToString("yyyyMMdd"), item.Data.Length);
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
