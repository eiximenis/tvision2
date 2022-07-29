using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Core.Engine;
using Tvision2.Xunit;
using Xunit;

namespace Tvision2.Core.Tests
{
    public class ComponentTree_Should : IClassFixture<TuiFixture<EmptySetupFixtureInitializer>>
    {
        private IComponentTree ComponentTree { get; }

        public ComponentTree_Should(TuiFixture<EmptySetupFixtureInitializer> fixture)
        {
            ComponentTree = fixture.GetOfType<IComponentTree>();
        }

        [Fact]
        public void Notify_Idle_Add_Cycles()
        {
            var count = 0;
            ComponentTree.OnAddingIdleCycle.AddOnce(_ => count++);


            Assert.Equal(1, count);

        }

    }
}
