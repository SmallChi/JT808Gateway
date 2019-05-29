using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808Reply
    {
        JT808Package Package { get; set; }
        byte[] HexData { get; set; }
        /// <summary>
        /// 根据实际情况适当调整包的大小
        /// </summary>
        int MinBufferSize { get; set; }
    }
}
