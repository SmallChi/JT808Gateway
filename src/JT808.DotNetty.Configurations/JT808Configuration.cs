using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Configurations
{
    public class JT808Configuration
    {
        public int Port { get; set; } = 808;

        public int UDPPort { get; set; } = 809;

        public int QuietPeriodSeconds { get; set; } = 1;

        public TimeSpan QuietPeriodTimeSpan => TimeSpan.FromSeconds(QuietPeriodSeconds);

        public int ShutdownTimeoutSeconds { get; set; } = 3;

        public TimeSpan ShutdownTimeoutTimeSpan => TimeSpan.FromSeconds(ShutdownTimeoutSeconds);

        public int SoBacklog { get; set; } = 8192;

        public int EventLoopCount { get; set; } = Environment.ProcessorCount;

        public int ReaderIdleTimeSeconds { get; set; } = 3600;

        public int WriterIdleTimeSeconds { get; set; } = 3600;

        public int AllIdleTimeSeconds { get; set; } = 3600;

        /// <summary>
        /// WebApi服务
        /// 默认828端口
        /// </summary>
        public int WebApiPort { get; set; } = 828;

        /// <summary>
        /// 源包分发器配置
        /// </summary>
        public List<JT808ClientConfiguration> SourcePackageDispatcherClientConfigurations { get; set; }

        /// <summary>
        /// 转发远程地址 (可选项)知道转发的地址有利于提升性能
        /// 按照808的消息，有些请求必须要应答，但是转发可以不需要有应答可以节省部分资源包括：
        //  1.消息的序列化
        //  2.消息的下发
        //  都有一定的性能损耗，那么不需要判断写超时 IdleState.WriterIdle
        //  就跟神兽貔貅一样。。。
        /// </summary>
        public List<JT808ClientConfiguration> ForwardingRemoteAddress { get; set; }

        public string RedisHost { get; set; }
    }
}
