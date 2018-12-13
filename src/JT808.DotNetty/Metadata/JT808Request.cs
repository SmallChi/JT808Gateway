using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Metadata
{
    public class JT808Request
    {
        public JT808Package Package { get; set; }

        public byte[] OriginalPackage { get;}

        public JT808Request()
        {

        }

        public JT808Request(JT808Package package, byte[] originalPackage)
        {
            Package = package;
            OriginalPackage = originalPackage;
        }
    }
}