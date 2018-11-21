using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dtos
{
    public class JT808DefaultResultDto: JT808ResultDto<string>
    {
        public JT808DefaultResultDto()
        {
            Data = "Hello,JT808 WebAPI";
            Code = 200;
        }
    }
}
