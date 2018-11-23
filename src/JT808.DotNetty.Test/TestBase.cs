using JT808.DotNetty.Configurations;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class TestBase
    {
        private IServiceCollection serviceDescriptors;

        public TestBase()
        {
            serviceDescriptors = new ServiceCollection();
        }
    }
}
