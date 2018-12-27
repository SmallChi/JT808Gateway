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
            /// <summary>
            /// 添加转发过滤地址
            /// </summary>
            public static string TransmitAdd = $"{RouteTablePrefix}/{TransmitPrefix}/Add";
            /// <summary>
            /// 删除转发过滤地址（不能删除在网关服务器配置文件配的地址）
            /// </summary>
            public static string TransmitRemove = $"{RouteTablePrefix}/{TransmitPrefix}/Remove";
            /// <summary>
            /// 获取转发过滤地址信息集合
            /// </summary>
            public static string TransmitGetAll = $"{RouteTablePrefix}/{TransmitPrefix}/GetAll";
            /// <summary>
            /// 获取Tcp包计数器
            /// </summary>
            public static string GetTcpAtomicCounter = $"{RouteTablePrefix}/GetTcpAtomicCounter";
            /// <summary>
            /// 基于Tcp的会话服务集合
            /// </summary>
            public static string SessionTcpGetAll = $"{RouteTablePrefix}/{SessionPrefix}/Tcp/GetAll";
            /// <summary>
            /// 基于Tcp的会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionTcpRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{SessionPrefix}/Tcp/RemoveByTerminalPhoneNo";
            /// <summary>
            /// 基于Tcp的统一下发信息
            /// </summary>
            public static string UnificationTcpSend = $"{RouteTablePrefix}/UnificationTcpSend";
            /// <summary>
            /// 获取Udp包计数器
            /// </summary>
            public static string GetUdpAtomicCounter = $"{RouteTablePrefix}/GetUdpAtomicCounter";
            /// <summary>
            /// 基于Udp的统一下发信息
            /// </summary>
            public static string UnificationUdpSend = $"{RouteTablePrefix}/UnificationUdpSend";
            /// <summary>
            /// 基于Udp的会话服务集合
            /// </summary>
            public static string SessionUdpGetAll = $"{RouteTablePrefix}/{SessionPrefix}/Udp/GetAll";
            /// <summary>
            /// 基于Udp的会话服务-通过设备终端号移除对应会话
            /// </summary>
            public static string SessionUdpRemoveByTerminalPhoneNo = $"{RouteTablePrefix}/{SessionPrefix}/Udp/RemoveByTerminalPhoneNo";

        }
    }
}
