using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core;
using Tvision2.DependencyInjection;
using Tvision2.Xunit;

namespace Tvision2.Controls.Tests
{
    public class TvControlsFixtureInitializer : StandardSetupFixtureInitializer<TestStartup>
    {
        protected override void CustomizeSetup(Tvision2Setup setup)
        {
            setup.AddTvControls();
        }
    }

}
