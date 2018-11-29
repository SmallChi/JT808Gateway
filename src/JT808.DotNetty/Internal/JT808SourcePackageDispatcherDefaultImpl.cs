using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Handlers;
using JT808.DotNetty.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// 原包分发器默认实现
    /// </summary>
    internal class JT808SourcePackageDispatcherDefaultImpl : IJT808SourcePackageDispatcher
    {
        private readonly JT808SourcePackageChannelService jT808SourcePackageChannelService;

        public JT808SourcePackageDispatcherDefaultImpl(JT808SourcePackageChannelService jT808SourcePackageChannelService)
        {
            this.jT808SourcePackageChannelService = jT808SourcePackageChannelService;
        }

        public async Task SendAsync(byte[] data)
        {
             await jT808SourcePackageChannelService.SendAsync(data);
        }
    }
}
