using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JT808.DotNetty.Dashbord.Controllers
{
    /// <summary>
    /// 统一信息下发
    /// </summary>
    [Route("jt808webapi/UnificationSend")]
    [ApiController]
    public class JT808UnificationSendController : ControllerBase
    {
        /// <summary>
        /// 文本信息下发
        /// </summary>
        /// <param name="terminalPhoneNo">终端设备号</param>
        /// <param name="text">下发文本信息</param>
        /// <returns></returns>
        [HttpGet("{terminalPhoneNo}/{text}")]
        public ActionResult<string> SendText(string terminalPhoneNo, string text)
        {
            
            return "value";
        }
    }
}
