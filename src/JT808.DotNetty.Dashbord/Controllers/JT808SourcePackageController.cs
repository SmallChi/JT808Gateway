using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.DotNetty.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JT808.DotNetty.Dashbord.Controllers
{
    /// <summary>
    /// 原包管理
    /// </summary>
    [Route("jt808webapi/SourcePackage")]
    [ApiController]
    public class JT808SourcePackageController : ControllerBase
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
        /// 获取原包通道信息集合
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<JT808SourcePackageChannelInfoDto>> GetAll()
        {
            return new List<JT808SourcePackageChannelInfoDto>() {
                new JT808SourcePackageChannelInfoDto {
                     Active=true,
                     Open=true,
                     Registered=true,
                     RemoteAddress="127.0.0.1:56"
                },
                new JT808SourcePackageChannelInfoDto {
                     Active=true,
                     Open=true,
                     Registered=true,
                     RemoteAddress="127.0.0.1:57"
                }
            };
        }
    }
}
