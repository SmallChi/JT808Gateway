using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JT808.DotNetty.Core.Metadata
{
    public class JT808HttpRequest
    {
        public string Json { get; set; }

        public JT808HttpRequest()
        {

        }

        public JT808HttpRequest(string json)
        {
            Json = json;
        }
    }
}