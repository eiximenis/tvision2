using System;
using Tvision2.Xunit;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Tvision2.Controls.Tests
{
    public class ConntrolsTree_Should : IClassFixture<TuiFixture<TvControlsFixtureInitializer>>
    {
        private readonly TuiFixture<TvControlsFixtureInitializer> _context;
        private  IControlsTree? _controlsTree;
        public ConntrolsTree_Should(TuiFixture<TvControlsFixtureInitializer> context)
        {
            _context = context;
            _context.Run(sp =>
            {
                _controlsTree = sp.GetService<IControlsTree>();
            });
        }


        [Fact]
        public void Add_Controls_On_Root()
        {
            
            Assert.False(true);
        }
    }
}
