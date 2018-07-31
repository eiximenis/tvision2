using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tvision2.Core;
using Tvision2.Dialogs;

namespace Tvision2.DependencyInjection
{
    public static class Tvision2SetupExtensions_Dialogs
    {
        public static Tvision2Setup AddTvDialogs(this Tvision2Setup setup)
        {
            setup.Builder.ConfigureServices(sc =>
            {
                sc.AddSingleton<IDialogManager, DialogManager>();
            });

            return setup;
        }
    }
}
