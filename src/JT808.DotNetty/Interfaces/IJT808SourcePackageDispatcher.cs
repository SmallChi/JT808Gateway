using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Interfaces
{
    /// <summary>
    /// 源包分发器
    /// </summary>
    public interface IJT808SourcePackageDispatcher
    {
        Task SendAsync(byte[] data);
    }
}
