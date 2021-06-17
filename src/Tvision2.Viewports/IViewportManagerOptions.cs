using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Viewports
{
    public interface IViewportManagerOptions
    {
        void EnableDynamicViewports();
    }

    class ViewportManagerOptions : IViewportManagerOptions
    {

        public bool DynamicViewportsEnabled { get; private set; }
        public ViewportManagerOptions()
        {
            DynamicViewportsEnabled = false;
        }

        void IViewportManagerOptions.EnableDynamicViewports()
        {
            DynamicViewportsEnabled = true;
        }
    }
}
