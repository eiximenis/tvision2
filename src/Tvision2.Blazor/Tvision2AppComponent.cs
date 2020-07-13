using BlazorTerm;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tvision2.ConsoleDriver.BlazorTerm;
using Tvision2.Core;
using Tvision2.Core.Engine;
using Tvision2.Engine.Console;

namespace Tvision2.Blazor
{
    public class Tvision2AppComponent :  ComponentBase
    {   
        [Inject]
        public ITuiEngine TuiEngine { get; set; }
        [Inject]
        public Tvision2Options TvOptions { get; set; }


        protected BTermComponent terminal;

        protected override bool ShouldRender() => false;


        public Tvision2AppComponent()
        {
            
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                terminal.CanvasReady += OnTerminalReady;
            }

            return Task.CompletedTask;
        }

        private void OnTerminalReady(object sender, EventArgs e)
        {
            ((BtermConsoleDriver)TuiEngine.ServiceProvider.GetService(typeof(IConsoleDriver))).BoundToBterm(terminal);
            TuiEngine.Start(CancellationToken.None);
        }

    }
}
