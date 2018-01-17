using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components.Props
{
    public abstract class TvProperty 
    {
        public string Name { get; }

        public Type Type { get; }
        public abstract bool IsDirty { get; }

        public TvProperty(string name, Type type)
        {
            Name = name;
            Type = type;
        }

    }

}
