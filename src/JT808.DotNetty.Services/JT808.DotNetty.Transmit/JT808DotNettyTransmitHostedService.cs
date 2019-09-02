using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Handlers.Logging;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JT808.DotNetty.Transmit.Configs;
using System.Linq;
using JT808.DotNetty.Transmit.Handlers;
using JT808.DotNetty.Abstractions;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace JT808.DotNetty.Transmit
{
    public class JT808DotNettyTransmitHostedService:IHostedService
    {
        private readonly JT808DotNettyTransmitService jT808DotNettyTransmitService;
        private readonly IJT808MsgConsumer jT808MsgConsumer;
        public JT808DotNettyTransmitHostedService(
            IJT808MsgConsumer jT808MsgConsumer,
            JT808DotNettyTransmitService jT808DotNettyTransmitService)
        {
            this.jT808DotNettyTransmitService = jT808DotNettyTransmitService;
            this.jT808MsgConsumer = jT808MsgConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            jT808MsgConsumer.Subscribe();
            jT808MsgConsumer.OnMessage(item=> {
                 jT808DotNettyTransmitService.SendAsync(item.TerminalNo,item.Data);
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
