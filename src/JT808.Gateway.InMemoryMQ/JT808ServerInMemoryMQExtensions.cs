using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JT808.Gateway.InMemoryMQ
{
    public static class JT808ServerInMemoryMQExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerInMemoryMQ(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgProducer, JT808MsgProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer>();
            return jT808GatewayBuilder;
        }
    }
}