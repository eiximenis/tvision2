using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex.Controls
{
    public interface IStatexCommandActionFilter <TControlState, TCommandArg>
    {
        void When(Func<TCommandArg, bool> predicate);
    }
}
