using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Enums
{
    public enum JT808UseType : byte
    {
        /// <summary>
        /// 使用正常方式
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 使用队列方式
        /// </summary>
        Queue = 2
    }
}
