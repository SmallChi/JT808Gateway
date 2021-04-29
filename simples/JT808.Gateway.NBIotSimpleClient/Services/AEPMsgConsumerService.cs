using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Services
{
    public class AEPMsgConsumerService : IHostedService
    {
        ILogger Logger;

        const string pulsarMqTenant = "aep-msgpush";

        IConsumer<ReadOnlySequence<byte>> pulsarConsumer;

        public AEPMsgConsumerService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<AEPMsgConsumerService>();
            //自己填写
            string topic = "test";
            //自己填写
            string tenantId = "";
            //自己填写
            string token = "";
            var  pulsarClient = PulsarClient.Builder().ServiceUrl(new Uri($"pulsar+ssl://msgpush.ctwing.cn:16651"))
                                            .AuthenticateUsingToken(token)
                                            .Build();
            pulsarConsumer = pulsarClient.NewConsumer()
                     .SubscriptionName(tenantId)
                     .Topic($"{pulsarMqTenant}/{tenantId}/{topic}")
                     .InitialPosition(SubscriptionInitialPosition.Earliest)
                     .SubscriptionType(SubscriptionType.Shared)
                     .Create();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async() => 
            {
                await foreach (var message in pulsarConsumer.Messages(cancellationToken))
                {
                    Logger.LogDebug("Received: " + Encoding.UTF8.GetString(message.Data.ToArray()));
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            pulsarConsumer.Unsubscribe();
            return Task.CompletedTask;
        }
    }
}
