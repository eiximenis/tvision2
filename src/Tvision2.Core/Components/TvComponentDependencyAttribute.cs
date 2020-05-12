using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Components
{

    public enum DependencyBinding
    {
        IfAlreadyCreated = 0,
        WhenCreate = 1
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TvComponentDependencyAttribute : Attribute
    {
        public TvComponentDependencyAttribute()
        {
            Binding = DependencyBinding.IfAlreadyCreated;
        }
        public string Name { get; set; }

        public DependencyBinding Binding { get; set; }

    }
}
