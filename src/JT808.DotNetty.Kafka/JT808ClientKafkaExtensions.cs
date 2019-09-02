using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JT808.DotNetty.Kafka
{
    public static class JT808ClientKafkaExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="configuration">GetSection("JT808MsgConsumerConfig")</param>
        /// <returns></returns>
        public static IServiceCollection AddJT808ClientKafkaMsgConsumer(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.Configure<JT808MsgConsumerConfig>(configuration.GetSection("JT808MsgConsumerConfig"));
            serviceDescriptors.TryAddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            return serviceDescriptors;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="configuration">GetSection("JT808MsgReplyProducerConfig")</param>
        /// <returns></returns>
        public static IServiceCollection AddJT808ClientKafkaMsgReplyProducer(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.Configure<JT808MsgReplyProducerConfig>(configuration.GetSection("JT808MsgReplyProducerConfig"));
            serviceDescriptors.TryAddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            return serviceDescriptors;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="configuration">GetSection("JT808SessionConsumerConfig")</param>
        /// <returns></returns>
        public static IServiceCollection AddJT808ClientKafkaSessionConsumer(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.Configure<JT808SessionConsumerConfig>(configuration.GetSection("JT808SessionConsumerConfig"));
            serviceDescriptors.TryAddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
            return serviceDescriptors;
        }
    }
}