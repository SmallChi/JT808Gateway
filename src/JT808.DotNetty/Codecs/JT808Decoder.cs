﻿using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.DotNetty.Internal;
using JT808.DotNetty.Interfaces;

namespace JT808.DotNetty.Codecs
{
    /// <summary>
    /// JT808解码
    /// </summary>
    internal class JT808Decoder : ByteToMessageDecoder
    {
        private readonly ILogger<JT808Decoder> logger;

        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;
        
        public JT808Decoder(
            JT808AtomicCounterService jT808AtomicCounterService,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            ILoggerFactory loggerFactory)
        {
            this.jT808AtomicCounterService = jT808AtomicCounterService;
            this.logger = loggerFactory.CreateLogger<JT808Decoder>();
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] buffer = new byte[input.Capacity + 2];
            try
            {
                input.ReadBytes(buffer, 1, input.Capacity);
                buffer[0] = JT808Package.BeginFlag;
                buffer[input.Capacity + 1] = JT808Package.EndFlag;
                jT808SourcePackageDispatcher?.SendAsync(buffer);
                JT808Package jT808Package = JT808Serializer.Deserialize<JT808Package>(buffer);
                output.Add(jT808Package);
                jT808AtomicCounterService.MsgSuccessIncrement();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
                } 
            }
            catch (JT808.Protocol.Exceptions.JT808Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + buffer);
                }
            }
            catch (Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + buffer);
                }
            }
        }
    }
}