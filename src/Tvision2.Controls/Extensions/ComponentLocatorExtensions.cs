using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tvision2.Controls;

namespace Tvision2.Core.Engine
{
    public static class ComponentLocatorExtensions
    {
        public static TvControlMetadata GetParentControl(this ComponentLocator locator)
        {
            var parent = locator.GetParentNode();
            return parent?.GetTag<TvControlMetadata>();
        }

        public static IEnumerable<TvControlMetadata> DescendantControls(this ComponentLocator locator)
        {
            return locator.DescendantNodes().Where(n => n.HasTag<TvControlMetadata>()).Select(n => n.GetTag<TvControlMetadata>());
        }
    }
}
