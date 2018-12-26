using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808HttpResponse
    {
        public byte[] Data { get; set; }

        public JT808HttpResponse()
        {

        }

        public JT808HttpResponse(byte[] data)
        {
            this.Data = data;
        }
    }
}