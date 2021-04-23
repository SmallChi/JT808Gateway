using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Metadata
{
    public class DeviceInfo
    {
        /// <summary>
        /// 终端手机号
        /// </summary>
        public string TerminalPhoneNo { get; set; }
        /// <summary>
        /// 鉴权码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool Successed { get; set; }
    }
}
