using JT808.DotNetty.Abstractions.Enums;
using System;
using System.Collections.Concurrent;

namespace JT808.DotNetty.Core.Services
{
    public class JT808AtomicCounterServiceFactory
    {
        private readonly ConcurrentDictionary<JT808TransportProtocolType, JT808AtomicCounterService> cache;

        public JT808AtomicCounterServiceFactory()
        {
            cache = new ConcurrentDictionary<JT808TransportProtocolType, JT808AtomicCounterService>();
        }

        public JT808AtomicCounterService Create(JT808TransportProtocolType type)
        {
            if(cache.TryGetValue(type,out var service))
            {
                return service;
            }
            else
            {
                var serviceNew = new JT808AtomicCounterService();
                cache.TryAdd(type, serviceNew);
                return serviceNew;
            }
        }
    }
}
