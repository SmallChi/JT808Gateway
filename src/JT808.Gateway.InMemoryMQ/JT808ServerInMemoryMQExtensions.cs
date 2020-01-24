using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JT808.Gateway.InMemoryMQ
{
    public static class JT808ServerInMemoryMQExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddJT808ServerInMemoryMQ(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMsgService>();
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgProducer), typeof(JT808MsgProducer), ServiceLifetime.Singleton));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgConsumer), typeof(JT808MsgConsumer), ServiceLifetime.Singleton));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyProducer), typeof(JT808MsgReplyProducer), ServiceLifetime.Singleton));
            jT808GatewayBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(JT808MsgReplyConsumer), ServiceLifetime.Singleton));
            return jT808GatewayBuilder;
        }
    }
}