using JT808.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Services
{
    /// <summary>
    /// 接收平台下发的数据包
    /// </summary>
    public  class ReceviePackageService
    {
        public BlockingCollection<JT808Package> BlockingCollection { get; }

        public ReceviePackageService()
        {
            BlockingCollection = new BlockingCollection<JT808Package>(999999);
        }
    }
}
