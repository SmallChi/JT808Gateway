using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JT808.DotNetty.Core.Metadata
{
    public  class JT808UdpPackage
    {
        public JT808UdpPackage(byte[] buffer, EndPoint sender)
        {
            Buffer = buffer;
            Sender = sender;
        }

        public byte[] Buffer { get; }

        public EndPoint Sender { get; }
    }
}
