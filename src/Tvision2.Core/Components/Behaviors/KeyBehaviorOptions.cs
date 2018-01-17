using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tvision2.Core.Components.Behaviors
{
    public class KeyBehaviorOptions
    {
        public IEnumerable<ConsoleKey> Keys { get; set; }
        public Action<ConsoleKey> Action { get; set; }
    }
}
