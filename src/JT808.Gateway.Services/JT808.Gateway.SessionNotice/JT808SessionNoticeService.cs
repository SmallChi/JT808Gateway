using Microsoft.Extensions.Logging;

namespace JT808.Gateway.SessionNotice
{
    public class JT808SessionNoticeService
    {
        protected ILogger logger { get; }
        public JT808SessionNoticeService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JT808SessionNoticeService>();
        }
        public virtual void Processor((string Notice, string TerminalNo) parameter)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"{parameter.Notice}-{parameter.TerminalNo}");
            }
        }
    }
}
