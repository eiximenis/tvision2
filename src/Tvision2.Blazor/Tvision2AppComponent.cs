using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tvision2.ConsoleDriver.Blazor;
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

        protected string Id { get; }

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        protected ElementReference _terminal;


        protected override bool ShouldRender() => false;


        public Tvision2AppComponent()
        {
            Id = $"tv_{Guid.NewGuid()}";
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ((BtermConsoleDriver)TuiEngine.ServiceProvider.GetService(typeof(IConsoleDriver))).BindToTerminal(Id, JsRuntime);
                TuiEngine.Start(CancellationToken.None);
            }

            return Task.CompletedTask;
        }

    }
}
