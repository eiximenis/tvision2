using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Engine
{
    public class TuiEngineBuilder
    {
        private readonly Dictionary<string, object> _customItems;

        public TuiEngineBuilder()
        {
            _customItems = new Dictionary<string, object>();
        }

        public TuiEngine Build()
        {
            return new TuiEngine(_customItems);
        }

        public void SetCustomItem(string key, object item)
        {
            _customItems.Add(key, item);
        }
    }
}
