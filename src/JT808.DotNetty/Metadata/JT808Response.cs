using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Metadata
{
    public class JT808Response
    {
        public JT808Package Package { get; set; }

        public JT808Response()
        {

        }
        public JT808Response(JT808Package package)
        {
            Package = package;
        }
    }
}