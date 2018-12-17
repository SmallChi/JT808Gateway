using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.DotNetty.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JT808.DotNetty.Dashbord.Controllers
{
    /// <summary>
    /// 转发管理
    /// </summary>
    [Route("jt808webapi/Transmit")]
    [ApiController]
    public class JT808TransmitController : ControllerBase
    {
        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public ActionResult<bool> Add([FromBody]JT808IPAddressDto jT808IPAddressDto)
        {
            return true;
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Remove")]
        public ActionResult<bool> Remove([FromBody]JT808IPAddressDto jT808IPAddressDto)
        {
            return true;
        }

        /// <summary>
        /// 获取转发IP地址集合
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<string>> GetAll()
        {
            return new List<string>() { "127.0.0.1:80", "127.0.0.1:81" };
        }
    }
}
