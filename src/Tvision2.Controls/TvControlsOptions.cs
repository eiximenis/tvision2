using System;
using System.Data.Common;
using Tvision2.DependencyInjection;
using Tvision2.Styles;

namespace Tvision2.Controls
{
    class TvControlsOptions : IControlsOptions
    {
        public Action<ISkinManagerBuilder> SkinOptions { get; private set; }
        public bool MouseManagerEnabled { get; private set; }

        public TvControlsOptions()
        {
            MouseManagerEnabled = false;
            SkinOptions = null;
        }
        
        IControlsOptions IControlsOptions.ConfigureSkins(Action<ISkinManagerBuilder> options)
        {
            SkinOptions = options;
            return this;
        }

        IControlsOptions IControlsOptions.EnableMouseManager()
        {
            MouseManagerEnabled = true;    
            return this;
        }
    }
}