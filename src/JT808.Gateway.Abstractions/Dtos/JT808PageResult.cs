using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Abstractions.Dtos
{
    public class JT808PageResult<T>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; } = 0;
        public T Data { get; set; }
    }
}
