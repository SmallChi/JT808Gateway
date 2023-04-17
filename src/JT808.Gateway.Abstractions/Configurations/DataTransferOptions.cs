using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions.Configurations
{
    public class DataTransferOptions
    {
        public string Host { get; set; }

        public List<string> TerminalNos { get; set; }
    }
}
