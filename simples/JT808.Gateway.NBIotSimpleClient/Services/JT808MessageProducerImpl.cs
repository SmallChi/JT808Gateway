using JT808.Gateway.Client;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Services
{
    public class JT808MessageProducerImpl : IJT808MessageProducer
    {
        ReceviePackageService ReceviePackageService;
        public JT808MessageProducerImpl(
            ReceviePackageService receviePackageService)
        {
            ReceviePackageService = receviePackageService;
        }
        public ValueTask ProduceAsync(JT808Package package)
        {
            ReceviePackageService.BlockingCollection.Add(package);
            return default;
        }
        public void Dispose()
        {

        }
    }
}
