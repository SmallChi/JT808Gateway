using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.DotNetty.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JT808.DotNetty.Dashbord.Controllers
{
    /// <summary>
    /// 计数器管理
    /// </summary>
    [Route("jt808webapi/AtomicCounter")]
    [ApiController]
    public class JT808AtomicCounterController : ControllerBase
    {
        /// <summary>
        /// 获取包计算器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAtomicCounter")]
        public ActionResult<JT808AtomicCounterDto> GetAtomicCounter()
        {
            return new JT808AtomicCounterDto {
                 MsgSuccessCount=100000,
                  MsgFailCount=0
            };
        }
    }
}
