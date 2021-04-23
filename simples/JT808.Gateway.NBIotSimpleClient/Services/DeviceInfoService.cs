using JT808.Gateway.NBIotSimpleClient.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Services
{
    public  class DeviceInfoService
    {
        public ConcurrentDictionary<string,DeviceInfo> DeviceInfos { get; set; }

        public DeviceInfoService()
        {
            DeviceInfos = new ConcurrentDictionary<string, DeviceInfo>();
        }
    }
}
