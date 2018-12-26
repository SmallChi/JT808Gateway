using JT808.DotNetty.Abstractions;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Impls
{
    /// <summary>
    /// 原包分发器默认空实现
    /// </summary>
    public class JT808SourcePackageDispatcherEmptyImpl : IJT808SourcePackageDispatcher
    {
        public  Task SendAsync(byte[] data)
        {
            return Task.CompletedTask;
        }
    }
}
