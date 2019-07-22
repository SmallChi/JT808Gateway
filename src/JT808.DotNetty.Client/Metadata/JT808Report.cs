using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Client.Metadata
{
    public class JT808Report
    {
        public long SendTotalCount { get; set; }
        public long ReceiveTotalCount { get; set; }
        public DateTime CurrentDate { get; set; }
        public int Connections { get; set; }
        public int OnlineConnections { get; set; }
        public int OfflineConnections { get; set; }
    }
}
