using JT808.Protocol;
using JT808.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Client.Metadata
{
    public class JT808ClientRequest
    {
        public JT808Package Package { get; }

        public byte[] HexData { get; }

        public JT808Version Version { get; }

        /// <summary>
        /// 根据实际情况适当调整包的大小
        /// </summary>
        public int MinBufferSize { get;}

        public JT808ClientRequest(JT808Package package, JT808Version version= JT808Version.JTT2013, int minBufferSize=1024)
        {
            Package = package;
            MinBufferSize = minBufferSize;
            Version = version;
        }

        public JT808ClientRequest(byte[] hexData)
        {
            HexData = hexData;
        }
    }
}