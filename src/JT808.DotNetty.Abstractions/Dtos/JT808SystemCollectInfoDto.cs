using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Abstractions.Dtos
{
    public class JT808SystemCollectInfoDto
    {
        /// <summary>
        /// 进程Id
        /// </summary>
        public int ProcessId { get; set; }
        /// <summary>
        /// 进程分配内存
        /// 单位MB
        /// </summary>
        public double WorkingSet64 { get; set; }
        /// <summary>
        /// 进程分配内存峰值
        /// 单位MB
        /// </summary>
        public double PeakWorkingSet64 { get; set; }
        /// <summary>
        /// 进程分配私有内存
        /// 单位MB
        /// </summary>
        public double PrivateMemorySize64 { get; set; }
        /// <summary>
        /// 进程执行CPU总处理时间
        /// </summary>
        public TimeSpan CPUTotalProcessorTime { get; set; }
    }
}
