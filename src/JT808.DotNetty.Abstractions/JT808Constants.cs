namespace JT808.DotNetty.Abstractions
{
    public static class JT808Constants
    {
        public const string SessionOnline= "JT808SessionOnline";

        public const string SessionOffline = "JT808SessionOffline";

        public static class JT808WebApiRouteTable
        {
            public const string RouteTablePrefix = "/jt808api";

            public const string SessionPrefix = "Session";

            public const string TransmitPrefix = "Transmit";

            public const string TrafficPrefix = "Traffic";

            public const string TcpPrefix = "Tcp";

            public const string UdpPrefix = "Udp";

            /// <summary>
            ///基于Tcp的添加转发过滤地址
            /// </summary>
            public static string TransmitAdd = $"{RouteTablePrefix}/{TcpPrefix}/{TransmitPrefix}/Add";
            /// <summary>
            /// 基于Tcp的删除转发过滤地址（不能删除在网关服务器配置文件配的地址）
            /// </summary>
            public static string TransmitRemove = $"{RouteTablePrefix}/{TcpPrefix}/{TransmitPrefix}/Remove";
            /// <summary>
            ///基于Tcp的获取转发过滤地址信息集合
            /// </summary>
            public static string TransmitGetAll = $"{RouteTablePrefix}/{TcpPrefix}/{TransmitPrefix}/GetAll";
            /// <summary>
            /// 基于Tcp的包计数器
            /// </summary>
            public static string GetTcpAtomicCounter = $"{RouteTablePrefix}/{TcpPrefix}/GetAtomicCounter";
            /// <summary>
            /// 基于Tcp的会话服务集合
            /// </summary>
            public static string SessionTcpGetAll = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/GetAll";
            /// <summary>
            /// 基于Tcp的会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionTcpRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{TcpPrefix}/{SessionPrefix}/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 基于Tcp的统一下发信息
            /// </summary>
            public static string UnificationTcpSend = $"{RouteTablePrefix}/{TcpPrefix}/UnificationSend";
            /// <summary>
            /// 基于Tcp的流量服务获取
            /// </summary>
            public static string TrafficTcpGet = $"{RouteTablePrefix}/{TcpPrefix}/{TrafficPrefix}/Get";

            /// <summary>
            /// 获取Udp包计数器
            /// </summary>
            public static string GetUdpAtomicCounter = $"{RouteTablePrefix}/{UdpPrefix}/GetAtomicCounter";
            /// <summary>
            /// 基于Udp的统一下发信息
            /// </summary>
            public static string UnificationUdpSend = $"{RouteTablePrefix}/{UdpPrefix}/UnificationSend";
            /// <summary>
            /// 基于Udp的会话服务集合
            /// </summary>
            public static string SessionUdpGetAll = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/GetAll";
            /// <summary>
            /// 基于Udp的会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionUdpRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{UdpPrefix}/{SessionPrefix}/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 基于Udp的流量服务获取
            /// </summary
            public static string TrafficUdpGet = $"{RouteTablePrefix}/{UdpPrefix}/{TrafficPrefix}/Get";
        }
    }
}
