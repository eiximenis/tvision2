using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Core.Styles;
using Tvision2.Statex;
using Tvision2.Statex.Behaviors;

namespace Tvision2.Sample
{

    class TestComponent : TvComponent<string>
    {
        public TestComponent(AppliedStyle style, string initialState, string name = null) : base(style, initialState, name)
        {
        }

    }


    class TestBehavior : StoreBehavior<string>
    {
        public TestBehavior(ITvStoreSelector storeSelector) : base(storeSelector)
        {
        }

        protected override bool Update(BehaviorContext<string> updateContext, ITvStoreSelector storeSelector)
        {
            return false;
        }
    }
}
