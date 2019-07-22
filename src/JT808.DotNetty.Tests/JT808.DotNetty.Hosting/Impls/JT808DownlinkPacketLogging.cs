using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Abstractions.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JT808.Protocol.Extensions;

namespace JT808.DotNetty.Hosting.Impls
{
    public class JT808DownlinkPacketLogging : IJT808DownlinkPacket
    {
        private readonly ILogger logger;
        public JT808DownlinkPacketLogging(ILoggerFactory loggerFactory )
        {
            logger = loggerFactory.CreateLogger("JT808DownlinkPacketLogging");
        }
        public Task ProcessorAsync(byte[] data, JT808TransportProtocolType transportProtocolType)
        {
           logger.LogInformation("send >>>"+data.ToHexString());
           return Task.CompletedTask;
        }
    }
}
