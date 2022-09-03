namespace JT808.Gateway.Abstractions
{
    public static class JT808GatewayConstants
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
            /// 基于Tcp的会话服务集合
            /// </summary>
            public static string SessionTcpGetAll = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/GetAll";            
            /// <summary>
            /// 基于Tcp的会话服务集合
            /// </summary>
            public static string SessionTcpByPage = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/SessionTcpByPage";
            /// <summary>
            /// 会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 会话服务-通过设备终端号查询对应会话
            /// </summary>
            public static string QueryTcpSessionByTerminalPhoneNo = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/QuerySessionByTerminalPhoneNo";
            /// <summary>
            /// 统一下发信息
            /// </summary>
            public static string UnificationSend = $"{RouteTablePrefix}/UnificationSend";
            /// <summary>
            /// 基于Udp的虚拟会话服务集合
            /// </summary>
            public static string SessionUdpGetAll = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/GetAll";            
            /// <summary>
            /// 基于Udp的虚拟会话服务集合
            /// </summary>
            public static string SessionUdpByPage = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/SessionUdpByPage";
            /// <summary>
            /// 会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string RemoveUdpByTerminalPhoneNo = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 会话服务-通过设备终端号查询对应会话
            /// </summary>
            public static string QueryUdpSessionByTerminalPhoneNo = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/QuerySessionByTerminalPhoneNo";
            /// <summary>
            /// 黑名单添加
            /// </summary>
            public static string BlacklistAdd = $"{RouteTablePrefix}/Blacklist/Add";
            /// <summary>
            /// 黑名单删除
            /// </summary>
            public static string BlacklistRemove = $"{RouteTablePrefix}/Blacklist/Remove";
            /// <summary>
            /// 黑名单查询
            /// </summary>
            public static string BlacklistGet = $"{RouteTablePrefix}/Blacklist/GetAll";
        }
    }
}
