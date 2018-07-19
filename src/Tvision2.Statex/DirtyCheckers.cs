using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex
{
    public static class DirtyCheckers
    {
        public static bool ReferenceDirtyChecker<TState>(TState old, TState @new) where TState : class => old != @new;
    }
}
