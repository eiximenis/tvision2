using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTerm
{
    public class BTermComponent : ComponentBase
    {
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        protected override bool ShouldRender() => false;

        [Parameter]
        public int Rows{ get; set; }
        [Parameter]
        public int Columns { get; set; }

        // Should be calculated but there is NO easy way to calculate the needed height & width based on font and needed rows/cols
        [Parameter]
        public int Height { get; set; }
        // Should be calculated (easier than calculate height).
        [Parameter]
        public int Width { get; set; }

        protected ElementReference _canvas;

        public string Id { get; }  = Guid.NewGuid().ToString();

        public event EventHandler CanvasReady;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await BtermJsInterop.InitBTerm(JsRuntime, Id);
                CanvasReady?.Invoke(this, EventArgs.Empty);
            }
        }


        public void WriteAt(int x, int y, char character)
        {
            Debug.WriteLine($"+++ BTermComponent.cs -> DrawAt ({x},{y})->{character}");
            ((IJSInProcessRuntime)JsRuntime).InvokeVoid("bterm._.DrawAt", Id, x, y, BtermCharacter.FromCodePoint(character));
        }


    }
}
