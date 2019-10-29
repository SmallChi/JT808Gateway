using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Abstractions.Dtos
{
    public class JT808ResultDto<T>
    {
        public JT808ResultDto()
        {
            Code = JT808ResultCode.Ok;
        }

        public string Message { get; set; }

        public int Code { get; set; }

        public T Data { get; set; }
    }

    public class JT808ResultCode
    {
        public const int Ok = 200;
        public const int Empty = 201;
        public const int AuthFail = 401;
        public const int NotFound = 404;
        public const int Fail = 400;
        public const int Error = 500;
    }
}
