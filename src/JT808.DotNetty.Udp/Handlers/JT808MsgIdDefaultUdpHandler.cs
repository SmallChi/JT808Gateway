using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;

namespace JT808.DotNetty.Udp.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    internal class JT808MsgIdDefaultUdpHandler : JT808MsgIdUdpHandlerBase
    {
        public JT808MsgIdDefaultUdpHandler(JT808UdpSessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
