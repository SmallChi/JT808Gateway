using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JT808.DotNetty.Metadata
{
    internal  class JT808UDPPackage
    {
        public JT808UDPPackage(byte[] buffer, EndPoint sender)
        {
            Buffer = buffer;
            Sender = sender;
        }

        public byte[] Buffer { get; }

        public EndPoint Sender { get; }
    }
}
