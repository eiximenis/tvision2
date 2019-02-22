using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Hooks;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{
    public interface IEventHookManager
    {
        Guid AddHook(IEventHook hookInstance);
        bool RemoveHook(Guid id);
    }
}
