using DotNetty.Codecs.Http;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808WebApiAuthorization
    {
        bool Authorization(IFullHttpRequest request, out IPrincipal principal);
    }
}
