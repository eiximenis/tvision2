using System;
using System.Collections.Generic;
using Tvision2.Controls;
using Tvision2.Controls.Button;
using Tvision2.Core.Components;
using Tvision2.Core.Engine;
using Tvision2.Core.Render;
using Tvision2.Layouts;
using Tvision2.Layouts.Canvas;
using Tvision2.Layouts.Grid;
using Tvision2.Layouts.Stack;
using Tvision2.Styles;

namespace Tvision2.Dialogs
{
    public class DialogState : IDirtyObject
    {
        public DialogState(ISkin skin, string prefix)
        {
        }

        internal void Init(TvDialog dialog, IComponentTree owner)
        {
   
            IsDirty = true;
        }

        internal void Destroy()
        {
        }

        public bool IsDirty { get; private set; }

        public void Validate() => IsDirty = false;
    }
}