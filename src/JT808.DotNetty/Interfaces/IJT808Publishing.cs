using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Interfaces
{
    public interface IJT808Publishing
    {
        Task Publish(string topicName,string key,string value);
    }
}
