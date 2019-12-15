using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.CleintBenchmark.Configs
{
    public class ClientBenchmarkOptions : IOptions<ClientBenchmarkOptions>
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public int DeviceCount { get; set; } = 10;
        /// <summary>
        /// 5000ms毫秒
        /// </summary>
        public int Interval { get; set; } = 5000;
        /// <summary>
        /// 需要多台机器同时访问，那么可以根据这个避开重复终端号
        /// 100000-200000-300000
        /// </summary>
        public int DeviceTemplate { get; set; } = 0;
        public ClientBenchmarkOptions Value =>this;
    }
}
