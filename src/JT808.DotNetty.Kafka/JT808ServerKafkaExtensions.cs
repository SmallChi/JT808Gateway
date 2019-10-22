using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JT808.DotNetty.Kafka
{
    public static class JT808ServerKafkaExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808NettyBuilder"></param>
        /// <param name="configuration">GetSection("JT808MsgProducerConfig")</param>
        /// <returns></returns>
        public static IJT808NettyBuilder AddJT808ServerKafkaMsgProducer(this IJT808NettyBuilder jT808NettyBuilder, IConfiguration configuration)
        {
            jT808NettyBuilder.JT808Builder.Services.Configure<JT808MsgProducerConfig>(configuration.GetSection("JT808MsgProducerConfig"));
            jT808NettyBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgProducer), typeof(JT808MsgProducer), ServiceLifetime.Singleton));
            return jT808NettyBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808NettyBuilder"></param>
        /// <param name="configuration">GetSection("JT808MsgReplyConsumerConfig")</param>
        /// <returns></returns>
        public static IJT808NettyBuilder AddJT808ServerKafkaMsgReplyConsumer(this IJT808NettyBuilder jT808NettyBuilder, IConfiguration configuration)
        {
            jT808NettyBuilder.JT808Builder.Services.Configure<JT808MsgReplyConsumerConfig>(configuration.GetSection("JT808MsgReplyConsumerConfig"));
            jT808NettyBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(JT808MsgReplyConsumer), ServiceLifetime.Singleton));
            return jT808NettyBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808NettyBuilder"></param>
        /// <param name="configuration">GetSection("JT808SessionProducerConfig")</param>
        /// <returns></returns>
        public static IJT808NettyBuilder AddJT808ServerKafkaSessionProducer(this IJT808NettyBuilder jT808NettyBuilder, IConfiguration configuration)
        {
            jT808NettyBuilder.JT808Builder.Services.Configure<JT808SessionProducerConfig>(configuration.GetSection("JT808SessionProducerConfig"));
            jT808NettyBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808SessionProducer), typeof(JT808SessionProducer), ServiceLifetime.Singleton));
            return jT808NettyBuilder;
        }
    }
}