using JT808.Gateway.SessionNotice;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SimpleQueueService.Impl
{
    public class JT808SessionNoticeServiceImpl : JT808SessionNoticeService
    {
        public JT808SessionNoticeServiceImpl(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public override void Processor((string Notice, string TerminalNo) parameter)
        {
            base.Processor(parameter);
        }
    }
}
