using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808Response
    {
        public JT808Package Package { get; set; }
        /// <summary>
        /// 根据实际情况适当调整包的大小
        /// </summary>
        public int MinBufferSize { get; set; } 

        public JT808Response()
        {

        }

        public JT808Response(JT808Package package, int minBufferSize = 1024)
        {
            Package = package;
            MinBufferSize = minBufferSize;
        }
    }
}