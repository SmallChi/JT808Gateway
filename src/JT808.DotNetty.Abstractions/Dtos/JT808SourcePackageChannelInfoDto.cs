using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Abstractions.Dtos
{
    /// <summary>
    /// 原包通道信息
    /// </summary>
    public class JT808SourcePackageChannelInfoDto
    {
        /// <summary>
        /// 远程地址
        /// </summary>
        public string RemoteAddress { get; set; }
        /// <summary>
        /// 本地地址
        /// </summary>
        public string LocalAddress { get; set; }
        /// <summary>
        /// 是否注册
        /// </summary>
        public bool Registered { get; set; }
        /// <summary>
        /// 是否活动
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool Open { get; set; }
    }
}
