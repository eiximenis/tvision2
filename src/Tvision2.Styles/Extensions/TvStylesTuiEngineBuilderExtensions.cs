using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Tvision2.Core;
using Tvision2.Core.Colors;
using Tvision2.Core.Engine;
using Tvision2.Styles;

namespace Tvision2.DependencyInjection
{
    public static class TvStylesTuiEngineBuilderExtensions
    {
        private const string stylesSetupStep = "TvStyles";
        public static Tvision2Setup AddStyles(this Tvision2Setup setup, Action<ISkinManagerBuilder>? skinOptions = null)
        {
            if (!setup.HasSetupStep(stylesSetupStep))           // Ensure typres are only registered once
            {
                setup.ConfigureServices(sc => sc.AddScoped<ISkinManager, SkinManager>());
                setup.ConfigureServices(sc => sc.AddScoped<StyledRenderContextAdatper>());
                setup.AddSetupStep(stylesSetupStep, new List<Action<ISkinManagerBuilder>>());
                setup.Options.AfterCreateInvoke((engine, sp) =>
                {
                    var skinManager = sp.GetRequiredService<ISkinManager>() as SkinManager;
                    var colorManager = sp.GetRequiredService<IColorManager>();
                    if (skinOptions != null)
                    {
                        FillSkinManager(setup.GetSetupStep<IEnumerable<Action<ISkinManagerBuilder>>>(stylesSetupStep), skinManager, colorManager);
                    }
                    else
                    {
                        FillDefaultSkinManager(skinManager, colorManager);
                    }

                    var styledAdapter = sp.GetRequiredService<StyledRenderContextAdatper>();
                    styledAdapter.AttachTo((ComponentTree)engine.UI);
                });
            }

            setup.GetSetupStep<List<Action<ISkinManagerBuilder>>>(stylesSetupStep).Add(skinOptions);

            return setup;
        }


        public static bool HasStylesEnabled(this Tvision2Setup setup)
        {
            return setup.HasSetupStep(stylesSetupStep);
        }

        private static void FillDefaultSkinManager(SkinManager sm, IColorManager cm)
        {
            // TODO: Create a default skin manager (single style black over white).
            var skinBuilder = new SkinManagerBuilder(cm);
            skinBuilder.AddDefaultSkin(sb => sb.AddBaseStyle(s => s.Default().DesiredStandard(sd => sd.UseForeground(TvColor.White).UseBackground(TvColor.Black))));
            skinBuilder.Fill(sm);
        }

        private static void FillSkinManager(IEnumerable<Action<ISkinManagerBuilder>> builderOptions, SkinManager sm, IColorManager cm)
        {
            var skinManagerBuilder = new SkinManagerBuilder(cm);
            foreach (var actionOpt in builderOptions)
            {
                actionOpt?.Invoke(skinManagerBuilder);
            }
            skinManagerBuilder.Fill(sm);
        }
    }
}
