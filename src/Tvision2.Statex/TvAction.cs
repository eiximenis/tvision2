using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex
{
    public class TvAction
    {
        public string Name { get;  }

        public TvAction(string name)
        {
            Name = name;
        }

        public TvAction<T> WithData<T>() => this as TvAction<T>;
   }

    public class TvAction<T> : TvAction
    {
        public T Value { get; }
        public TvAction(string name, T value) : base(name)
        {
            Value = value;
        }
    }
}
