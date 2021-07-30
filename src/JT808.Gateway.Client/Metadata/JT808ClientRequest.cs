using JT808.Protocol;
using JT808.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.Gateway.Client.Metadata
{
    public class JT808ClientRequest
    {
        public JT808Package Package { get; }

        public byte[] HexData { get; }

        /// <summary>
        /// 根据实际情况适当调整包的大小
        /// </summary>
        public int MinBufferSize { get;}


        public JT808ClientRequest(JT808Package package, int minBufferSize=1024)
        {
            Package = package;
            MinBufferSize = minBufferSize;
        }

        public JT808ClientRequest(byte[] hexData)
        {
            HexData = hexData;
        }
    }
}