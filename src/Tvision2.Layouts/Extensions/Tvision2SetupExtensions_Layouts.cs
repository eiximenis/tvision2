﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core;
using Tvision2.Layouts;

namespace Tvision2.Core.Engine
{
    public static class Tvision2SetupExtensions_Layouts
    {
        public static Tvision2Setup UseLayoutManager(this Tvision2Setup setup)
        {
            setup.ConfigureServices(sc =>
            {
 
                sc.AddScoped<ILayoutManager, LayoutManager>();
            });
            return setup;
        }

    } 
}
