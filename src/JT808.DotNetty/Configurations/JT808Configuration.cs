using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Configurations
{
    public class JT808Configuration
    {
        public int Port { get; set; } = 808;

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
        /// WebAPI服务
        /// 默认828端口
        /// </summary>
        public int WebAPIPort { get; set; } = 828;

        /// <summary>
        /// 源包分发器配置
        /// </summary>
        public List<JT808ClientConfiguration> SourcePackageDispatcherClientConfigurations { get; set; }
    }
}
