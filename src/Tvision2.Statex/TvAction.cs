using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Statex
{

    public enum TvActionExceptionHandling
    {
        Swallow,
        Rethrow
    }

    public class TvAction
    {
        public string Name { get;  }
        public TvActionExceptionHandling ExceptionHandler { get; set; }

        public TvAction(string name, TvActionExceptionHandling handler)
        {
            Name = name;
            ExceptionHandler = handler;
        }


        public TvAction<T> WithData<T>() => this as TvAction<T>;
   }

    public class TvAction<T> : TvAction
    {
        public T Value { get; }
        public TvAction(string name, T value, TvActionExceptionHandling handler = TvActionExceptionHandling.Rethrow) : base(name, handler )
        {
            Value = value;
        }
    }
}
