using System;
using Tvision2.Xunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Tvision2.Controls.Tests
{
    public class controltree_should : IClassFixture<TuiFixture<TvControlsFixtureInitializer>>
    {
        private readonly TuiFixture<TvControlsFixtureInitializer> _context;
        public controltree_should(TuiFixture<TvControlsFixtureInitializer> context)
        {
            _context = context;
        }


        [Fact]
        public void add_controls_on_root()
        {
            _context.Run(sp =>
            {
                var tree = sp.GetService<IControlsTree>();
   
            });
            Assert.False(true);
        }
    }
}
