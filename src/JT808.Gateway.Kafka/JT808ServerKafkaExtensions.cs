using JT808.Gateway.Configs.Kafka;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JT808.Gateway.Kafka
{
    public static class JT808ServerKafkaExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <param name="configuration">GetSection("JT808MsgProducerConfig")</param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerKafkaMsgProducer(this IJT808GatewayBuilder jT808GatewayBuilder, IConfiguration configuration)
        {
            jT808GatewayBuilder.JT808Builder.Services.Configure<JT808MsgProducerConfig>(configuration.GetSection("JT808MsgProducerConfig"));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgProducer), typeof(JT808MsgProducer), ServiceLifetime.Singleton));
            return jT808GatewayBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <param name="configuration">GetSection("JT808MsgReplyConsumerConfig")</param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerKafkaMsgReplyConsumer(this IJT808GatewayBuilder jT808GatewayBuilder, IConfiguration configuration)
        {
            jT808GatewayBuilder.JT808Builder.Services.Configure<JT808MsgReplyConsumerConfig>(configuration.GetSection("JT808MsgReplyConsumerConfig"));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(JT808MsgReplyConsumer), ServiceLifetime.Singleton));
            return jT808GatewayBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <param name="configuration">GetSection("JT808SessionProducerConfig")</param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerKafkaSessionProducer(this IJT808GatewayBuilder jT808GatewayBuilder, IConfiguration configuration)
        {
            jT808GatewayBuilder.JT808Builder.Services.Configure<JT808SessionProducerConfig>(configuration.GetSection("JT808SessionProducerConfig"));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808SessionProducer), typeof(JT808SessionProducer), ServiceLifetime.Singleton));
            return jT808GatewayBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <param name="configuration">GetSection("JT808MsgReplyLoggingProducerConfig")</param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerKafkaMsgReplyLoggingProducer(this IJT808GatewayBuilder jT808GatewayBuilder, IConfiguration configuration)
        {
            jT808GatewayBuilder.JT808Builder.Services.Configure<JT808MsgReplyLoggingProducerConfig>(configuration.GetSection("JT808MsgReplyLoggingProducerConfig"));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyLoggingProducer), typeof(JT808MsgReplyLoggingProducer), ServiceLifetime.Singleton));
            return jT808GatewayBuilder;
        }
    }
}