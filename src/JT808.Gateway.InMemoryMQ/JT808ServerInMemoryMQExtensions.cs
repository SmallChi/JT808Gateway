using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.InMemoryMQ.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.Gateway.InMemoryMQ.Test")]
namespace JT808.Gateway.InMemoryMQ
{
    public static class JT808ServerInMemoryMQExtensions
    {
        internal static List<JT808ConsumerType> ConsumerTypes { get; private set; }

        static JT808ServerInMemoryMQExtensions()
        {
            ConsumerTypes = new List<JT808ConsumerType>();
        }

        internal static JT808ConsumerType? ReplyMessageLoggingConsumer { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerInMemoryMQ(this IJT808GatewayBuilder jT808GatewayBuilder, JT808ConsumerType consumerType)
        {
            if ((consumerType & JT808ConsumerType.All) == JT808ConsumerType.All)
            {
                ConsumerTypes.Add(JT808ConsumerType.MsgIdHandlerConsumer);
                ConsumerTypes.Add(JT808ConsumerType.MsgLoggingConsumer);
                ConsumerTypes.Add(JT808ConsumerType.ReplyMessageConsumer);
                ConsumerTypes.Add(JT808ConsumerType.TrafficConsumer);
                ConsumerTypes.Add(JT808ConsumerType.TransmitConsumer);
                ConsumerTypes.Add(JT808ConsumerType.ReplyMessageLoggingConsumer);
            }
            else
            {
                if ((consumerType & JT808ConsumerType.MsgLoggingConsumer) == JT808ConsumerType.MsgLoggingConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.MsgLoggingConsumer);
                }
                if ((consumerType & JT808ConsumerType.MsgIdHandlerConsumer) == JT808ConsumerType.MsgIdHandlerConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.MsgIdHandlerConsumer);
                }
                if ((consumerType & JT808ConsumerType.ReplyMessageConsumer) == JT808ConsumerType.ReplyMessageConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.ReplyMessageConsumer);
                }
                if ((consumerType & JT808ConsumerType.TrafficConsumer) == JT808ConsumerType.TrafficConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.TrafficConsumer);
                }
                if ((consumerType & JT808ConsumerType.TransmitConsumer) == JT808ConsumerType.TransmitConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.TransmitConsumer);
                }
                if ((consumerType & JT808ConsumerType.ReplyMessageLoggingConsumer) == JT808ConsumerType.ReplyMessageLoggingConsumer)
                {
                    //
                    ReplyMessageLoggingConsumer = JT808ConsumerType.ReplyMessageLoggingConsumer;
                }
            }
            jT808GatewayBuilder.AddServerInMemoryConsumers();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808SessionService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgProducer, JT808MsgProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgConsumerFactory, JT808MsgConsumerFactory>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumerFactory, JT808MsgReplyConsumerFactory>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808MsgConsumerInMemoryHostedService>();
            return jT808GatewayBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddServerInMemoryMQ(this IJT808GatewayBuilder jT808GatewayBuilder,params JT808ConsumerType[] consumerTypes)
        {
            if (consumerTypes == null)
            {
                throw new ArgumentNullException("消费类型不为空!");
            }
            ConsumerTypes = consumerTypes.ToList();
            jT808GatewayBuilder.AddServerInMemoryConsumers();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMsgService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808SessionService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgProducer, JT808MsgProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgConsumerFactory, JT808MsgConsumerFactory>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumerFactory, JT808MsgReplyConsumerFactory>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808MsgConsumerInMemoryHostedService>();
            return jT808GatewayBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        internal static IServiceCollection AddServerInMemoryMQ(this IServiceCollection serviceDescriptors, JT808ConsumerType consumerType)
        {
            if ((consumerType & JT808ConsumerType.All) == JT808ConsumerType.All)
            {
                ConsumerTypes.Add(JT808ConsumerType.MsgIdHandlerConsumer);
                ConsumerTypes.Add(JT808ConsumerType.MsgLoggingConsumer);
                ConsumerTypes.Add(JT808ConsumerType.ReplyMessageConsumer);
                ConsumerTypes.Add(JT808ConsumerType.TrafficConsumer);
                ConsumerTypes.Add(JT808ConsumerType.TransmitConsumer);
            }
            else
            {
                if ((consumerType & JT808ConsumerType.MsgLoggingConsumer) == JT808ConsumerType.MsgLoggingConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.MsgLoggingConsumer);
                }
                if ((consumerType & JT808ConsumerType.MsgIdHandlerConsumer) == JT808ConsumerType.MsgIdHandlerConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.MsgIdHandlerConsumer);
                }
                if ((consumerType & JT808ConsumerType.ReplyMessageConsumer) == JT808ConsumerType.ReplyMessageConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.ReplyMessageConsumer);
                }
                if ((consumerType & JT808ConsumerType.TrafficConsumer) == JT808ConsumerType.TrafficConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.TrafficConsumer);
                }
                if ((consumerType & JT808ConsumerType.TransmitConsumer) == JT808ConsumerType.TransmitConsumer)
                {
                    ConsumerTypes.Add(JT808ConsumerType.TransmitConsumer);
                }
                if ((consumerType & JT808ConsumerType.ReplyMessageLoggingConsumer) == JT808ConsumerType.ReplyMessageLoggingConsumer)
                {
                    ReplyMessageLoggingConsumer = JT808ConsumerType.ReplyMessageLoggingConsumer;
                }
            }
            serviceDescriptors.AddServerInMemoryConsumers();
            serviceDescriptors.AddSingleton<JT808MsgService>();
            serviceDescriptors.AddSingleton<JT808ReplyMsgService>();
            serviceDescriptors.AddSingleton<JT808SessionService>();
            serviceDescriptors.AddSingleton<IJT808MsgProducer, JT808MsgProducer>();
            serviceDescriptors.AddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer>();
            serviceDescriptors.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
            serviceDescriptors.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
            serviceDescriptors.AddSingleton<IJT808MsgConsumerFactory, JT808MsgConsumerFactory>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyConsumerFactory, JT808MsgReplyConsumerFactory>();
            serviceDescriptors.AddHostedService<JT808MsgConsumerInMemoryHostedService>();
            return serviceDescriptors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        internal static IServiceCollection AddServerInMemoryMQ(this IServiceCollection  serviceDescriptors, params JT808ConsumerType[] consumerTypes)
        {
            if (consumerTypes == null)
            {
                throw new ArgumentNullException("消费类型不为空!");
            }
            ConsumerTypes = consumerTypes.ToList();
            serviceDescriptors.AddServerInMemoryConsumers();
            serviceDescriptors.AddSingleton<JT808MsgService>();
            serviceDescriptors.AddSingleton<JT808ReplyMsgService>();
            serviceDescriptors.AddSingleton<JT808SessionService>();
            serviceDescriptors.AddSingleton<IJT808MsgProducer, JT808MsgProducer>();
            serviceDescriptors.AddSingleton<IJT808MsgConsumer, JT808MsgConsumer>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer>();
            serviceDescriptors.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
            serviceDescriptors.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
            serviceDescriptors.AddSingleton<IJT808MsgConsumerFactory, JT808MsgConsumerFactory>();
            serviceDescriptors.AddSingleton<IJT808MsgReplyConsumerFactory, JT808MsgReplyConsumerFactory>();
            serviceDescriptors.AddHostedService<JT808MsgConsumerInMemoryHostedService>();
            return serviceDescriptors;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        private static IJT808GatewayBuilder AddServerInMemoryConsumers(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgIdHandlerService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgLoggingService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgReplyMessageService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgReplyMessageLoggingService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgTrafficService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgTransmitService>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgIdHandlerConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgLoggingConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgReplyMessageConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgTrafficConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgTransmitConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808MsgReplyMessageLoggingConsumer>();
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, IJT808MsgConsumer> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.MsgIdHandlerConsumer:
                            return factory.GetRequiredService<JT808MsgIdHandlerConsumer>();
                        case JT808ConsumerType.MsgLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgLoggingConsumer>();
                        case JT808ConsumerType.TrafficConsumer:
                            return factory.GetRequiredService<JT808MsgTrafficConsumer>();
                        case JT808ConsumerType.TransmitConsumer:
                            return factory.GetRequiredService<JT808MsgTransmitConsumer>();
                        case JT808ConsumerType.ReplyMessageConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageConsumer>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, IJT808MsgReplyConsumer> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.ReplyMessageLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageLoggingConsumer>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, JT808MsgServiceBase> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.MsgIdHandlerConsumer:
                            return factory.GetRequiredService<JT808MsgIdHandlerService>();
                        case JT808ConsumerType.MsgLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgLoggingService>();
                        case JT808ConsumerType.TrafficConsumer:
                            return factory.GetRequiredService<JT808MsgTrafficService>();
                        case JT808ConsumerType.TransmitConsumer:
                            return factory.GetRequiredService<JT808MsgTransmitService>();
                        case JT808ConsumerType.ReplyMessageConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageService>();
                        case JT808ConsumerType.ReplyMessageLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageLoggingService>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            return jT808GatewayBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        private static IServiceCollection AddServerInMemoryConsumers(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<JT808MsgIdHandlerService>();
            serviceDescriptors.AddSingleton<JT808MsgLoggingService>();
            serviceDescriptors.AddSingleton<JT808MsgReplyMessageService>();
            serviceDescriptors.AddSingleton<JT808MsgReplyMessageLoggingService>();
            serviceDescriptors.AddSingleton<JT808MsgTrafficService>();
            serviceDescriptors.AddSingleton<JT808MsgTransmitService>();
            serviceDescriptors.AddSingleton<JT808MsgIdHandlerConsumer>();
            serviceDescriptors.AddSingleton<JT808MsgLoggingConsumer>();
            serviceDescriptors.AddSingleton<JT808MsgReplyMessageConsumer>();
            serviceDescriptors.AddSingleton<JT808MsgTrafficConsumer>();
            serviceDescriptors.AddSingleton<JT808MsgTransmitConsumer>();
            serviceDescriptors.AddSingleton<JT808MsgReplyMessageLoggingConsumer>();
            serviceDescriptors.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, IJT808MsgConsumer> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.MsgIdHandlerConsumer:
                            return factory.GetRequiredService<JT808MsgIdHandlerConsumer>();
                        case JT808ConsumerType.MsgLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgLoggingConsumer>();
                        case JT808ConsumerType.TrafficConsumer:
                            return factory.GetRequiredService<JT808MsgTrafficConsumer>();
                        case JT808ConsumerType.TransmitConsumer:
                            return factory.GetRequiredService<JT808MsgTransmitConsumer>();
                        case JT808ConsumerType.ReplyMessageConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageConsumer>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            serviceDescriptors.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, IJT808MsgReplyConsumer> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.ReplyMessageLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageLoggingConsumer>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            serviceDescriptors.AddSingleton((factory) =>
            {
                Func<JT808ConsumerType, JT808MsgServiceBase> accesor = type =>
                {
                    switch (type)
                    {
                        case JT808ConsumerType.MsgIdHandlerConsumer:
                            return factory.GetRequiredService<JT808MsgIdHandlerService>();
                        case JT808ConsumerType.MsgLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgLoggingService>();
                        case JT808ConsumerType.TrafficConsumer:
                            return factory.GetRequiredService<JT808MsgTrafficService>();
                        case JT808ConsumerType.TransmitConsumer:
                            return factory.GetRequiredService<JT808MsgTransmitService>();
                        case JT808ConsumerType.ReplyMessageConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageService>();
                        case JT808ConsumerType.ReplyMessageLoggingConsumer:
                            return factory.GetRequiredService<JT808MsgReplyMessageLoggingService>();
                        default:
                            return default;
                    }
                };
                return accesor;
            });
            return serviceDescriptors;
        }
    }
}