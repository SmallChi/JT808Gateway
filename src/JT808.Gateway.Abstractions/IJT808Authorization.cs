using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808Authorization
    {
        bool Authorization(HttpListenerContext context, out IPrincipal principal);
    }
}
