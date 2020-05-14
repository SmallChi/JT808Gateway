namespace JT808.DotNetty.Abstractions
{
    public static class JT808NettyConstants
    {
        public const string SessionOnline= "JT808SessionOnline";

        public const string SessionOffline = "JT808SessionOffline";
        public const string SessionTopic = "jt808session";
        public const string MsgTopic = "jt808msgdefault";
        public const string MsgReplyTopic = "jt808msgreplydefault";

        public static class JT808WebApiRouteTable
        {
            public const string RouteTablePrefix = "/jt808api";

            public const string SessionPrefix = "Session";

            public const string TcpPrefix = "Tcp";

            public const string UdpPrefix = "Udp";

            /// <summary>
            /// 基于Tcp的包计数器
            /// </summary>
            public static string GetTcpAtomicCounter = $"{RouteTablePrefix}/{TcpPrefix}/GetAtomicCounter";
            /// <summary>
            /// 基于Tcp的会话服务集合
            /// </summary>
            public static string SessionTcpGetAll = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/GetAll";
            /// <summary>
            /// 会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{SessionPrefix}/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 会话服务-通过设备终端号查询对应会话
            /// </summary>
            public static string QueryTcpSessionByTerminalPhoneNo = $"{RouteTablePrefix}/{SessionPrefix}/QueryTcpSessionByTerminalPhoneNo";
            /// <summary>
            /// 统一下发信息
            /// </summary>
            public static string UnificationSend = $"{RouteTablePrefix}/UnificationSend";
            /// <summary>
            /// 获取Udp包计数器
            /// </summary>
            public static string GetUdpAtomicCounter = $"{RouteTablePrefix}/{UdpPrefix}/GetAtomicCounter";
            /// <summary>
            /// 基于Udp的会话服务集合
            /// </summary>
            public static string SessionUdpGetAll = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/GetAll";
        }
    }
}
