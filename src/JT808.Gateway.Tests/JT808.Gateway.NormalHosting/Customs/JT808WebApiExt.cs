using JT808.Gateway.Abstractions.Dtos;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using Microsoft.AspNetCore.Mvc;

namespace JT808.Gateway.NormalHosting.Customs
{
    [Route("jt808apiext")]
    [ApiController]
    public class JT808WebApiExt : ControllerBase
    {
        public JT808WebApiExt(JT808SessionManager jT808SessionManager, JT808BlacklistManager jT808BlacklistManager)
        {

        }

        /// <summary>
        /// index1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("index1")]
        public ActionResult<JT808ResultDto<string>> Index1()
        {
            JT808ResultDto<string> resultDto = new JT808ResultDto<string>();
            resultDto.Data = "Hello,JT808 WebApi Ext";
            resultDto.Code = JT808ResultCode.Ok;
            return resultDto;
        }
    }
}
