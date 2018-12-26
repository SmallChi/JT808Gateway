using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808Request
    {
        public JT808HeaderPackage Package { get; }

        /// <summary>
        /// 用于消息发送
        /// </summary>
        public byte[] OriginalPackage { get;}

        public JT808Request(JT808HeaderPackage package, byte[] originalPackage)
        {
            Package = package;
            OriginalPackage = originalPackage;
        }
    }
}