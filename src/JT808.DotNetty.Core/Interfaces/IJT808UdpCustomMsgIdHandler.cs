using JT808.DotNetty.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808UdpCustomMsgIdHandler
    {
        IJT808Reply Proccesser(JT808Request request);
    }

    public class JT808UdpCustomMsgIdHandlerEmpty : IJT808UdpCustomMsgIdHandler
    {
        public IJT808Reply Proccesser(JT808Request request)
        {
            return default;
        }
    }
}
