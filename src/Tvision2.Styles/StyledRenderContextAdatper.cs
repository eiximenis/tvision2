using System;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;

namespace Tvision2.Styles
{
    class StyledRenderContextAdatper
    {

        public ISkinManager SkinManager { get; }

        public StyledRenderContextAdatper(ISkinManager skinManager)
        {
            SkinManager = skinManager;
        }

        public void AttachTo(ComponentTree componentTree)
        {
            componentTree.ComponentMountingBegun += OnComponentMountingBegun;
        }

        private void OnComponentMountingBegun(object sender, TreeUpdatedEventArgs e)
        {
            e.Node.SetTag(this);
        }
    }



}