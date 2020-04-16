using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Xunit
{
    public interface ITuiFixtureInitializer
    {
        HostBuilder GetHostBuilder();
    }
}
