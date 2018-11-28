using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dashbord.Dtos
{
    public class JT808ResultDto<T>
    {
        public string Message { get; set; }

        public int Code { get; set; }

        public T Data { get; set; }
    }
}
