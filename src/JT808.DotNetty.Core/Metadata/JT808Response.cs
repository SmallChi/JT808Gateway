using JT808.DotNetty.Core.Interfaces;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808Response: IJT808Reply
    {
        public JT808Package Package { get; set; }
        public byte[] HexData { get; set; }
        public int MinBufferSize { get; set; } 

        public JT808Response()
        {

        }

        public JT808Response(JT808Package package, int minBufferSize = 1024)
        {
            Package = package;
            MinBufferSize = minBufferSize;
        }

        public JT808Response(byte[] hexData)
        {
            HexData = hexData;
        }
    }
}