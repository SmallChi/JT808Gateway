using JT808.DotNetty.Core.Handlers;

namespace JT808.DotNetty.WebApi.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    internal class JT808MsgIdDefaultWebApiHandler : JT808MsgIdHttpHandlerBase
    {
        private const string sessionRoutePrefix = "Session";

        private const string sourcePackagePrefix = "SourcePackage";

        private const string transmitPrefix = "Transmit";

        //1.TCP一套注入
        //2.UDP一套注入
        //3.统一的一套注入

        public JT808MsgIdDefaultWebApiHandler(

            )
        {

        }
    }
}
