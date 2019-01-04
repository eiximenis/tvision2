﻿using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{

    class GlobalStore : TvStore<GlobalState>
    {
        public GlobalStore() : base(new GlobalState())
        {

        }
        public GlobalStore(GlobalState state) : base(state)
        {
        }
    }
}
