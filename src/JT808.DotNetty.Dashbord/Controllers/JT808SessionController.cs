using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.DotNetty.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JT808.DotNetty.Dashbord.Controllers
{
    /// <summary>
    /// 会话管理
    /// </summary>
    [Route("jt808webapi/Session")]
    [ApiController]
    public class JT808SessionController : ControllerBase
    {
        /// <summary>
        /// 通过终端设备号删除会话
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveByTerminalPhoneNo")]
        public ActionResult<bool> RemoveByTerminalPhoneNo(string terminalPhoneNo)
        {
            return true;
        }

        /// <summary>
        /// 通过通道Id删除会话
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveByChannelId")]
        public ActionResult<bool> RemoveByChannelId(string channelId)
        {
            return true;
        }

        /// <summary>
        /// 获取会话集合
        /// </summary>
        /// <param name="jT808IPAddressDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<JT808SessionInfoDto>> GetAll()
        {
            return new List<JT808SessionInfoDto>() {
                new JT808SessionInfoDto {
                      ChannelId="0x00x0",
                      LastActiveTime=DateTime.Now,
                      RemoteAddressIP="127.0.0.1:559",
                      TerminalPhoneNo="123456789012",
                       StartTime=DateTime.Now,
                       WebApiPort=8091
                },
                new JT808SessionInfoDto {
                      ChannelId="0x00x1",
                      LastActiveTime=DateTime.Now,
                      RemoteAddressIP="127.0.0.1:558",
                      TerminalPhoneNo="123456789013",
                       StartTime=DateTime.Now,
                       WebApiPort=8092
                }
            };
        }
    }
}
