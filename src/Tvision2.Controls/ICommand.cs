using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Controls
{
    public interface ICommand { }
    public interface ICommand<TData> : ICommand
    { 
        Task<bool> Invoke(TData data);
    }
}
