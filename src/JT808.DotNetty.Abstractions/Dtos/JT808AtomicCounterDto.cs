namespace JT808.DotNetty.Abstractions.Dtos
{
    /// <summary>
    /// 包计数器服务
    /// </summary>
    public class JT808AtomicCounterDto
    {
        public long MsgSuccessCount { get; set; }

        public long MsgFailCount { get; set; }
    }
}
