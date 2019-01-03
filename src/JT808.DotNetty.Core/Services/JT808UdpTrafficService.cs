using JT808.DotNetty.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Services
{
    public class JT808UdpTrafficService
    {
        private readonly JT808AtomicCounter receiveCounter = new JT808AtomicCounter();

        private readonly JT808AtomicCounter sendCounter = new JT808AtomicCounter();

        public void ReceiveSize(long size)
        {
            receiveCounter.Add(size);
        }

        public long TotalReceiveSize
        {
            get { return receiveCounter.Count; }
        }

        public long TotalSendSize
        {
            get { return sendCounter.Count; }
        }

        public void SendSize(long size)
        {
            sendCounter.Add(size);
        }

        public void ResetSize()
        {
            receiveCounter.Reset();
            sendCounter.Reset();
        }
    }
}
