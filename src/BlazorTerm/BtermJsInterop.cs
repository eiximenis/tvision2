using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorTerm
{
    public class BtermJsInterop
    {
        
        public static ValueTask InitBTerm(IJSRuntime jSRuntime, string id)
        {
            return jSRuntime.InvokeVoidAsync("bterm.create", id);
        }

    }
}
