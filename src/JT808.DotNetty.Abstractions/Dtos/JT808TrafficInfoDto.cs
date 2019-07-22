using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Abstractions.Dtos
{
    public  class JT808TrafficInfoDto
    {
        /// <summary>
        /// 总接收大小 
        /// 单位KB
        /// </summary>
        public double TotalReceiveSize { get; set; }
        /// <summary>
        /// 总发送大小 
        /// 单位KB
        /// </summary>
        public double TotalSendSize { get; set; }
    }
}
