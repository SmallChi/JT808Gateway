using System.Threading.Tasks;

namespace JT808.DotNetty.Abstractions
{
    /// <summary>
    /// 源包分发器
    /// 自定义源包分发器业务
    /// ConfigureServices:
    /// services.Replace(new ServiceDescriptor(typeof(IJT808SourcePackageDispatcher),typeof(JT808SourcePackageDispatcherDefaultImpl),ServiceLifetime.Singleton));
    /// </summary>
    public interface IJT808SourcePackageDispatcher
    {
        Task SendAsync(byte[] data);
    }
}
