using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Services
{
    internal class JT808MsgService
    {
        public System.Collections.Concurrent.BlockingCollection<(string TerminalNo, byte[] Data)> MsgQueue { get; set; } = new System.Collections.Concurrent.BlockingCollection<(string TerminalNo, byte[] Data)>();
    }
}
