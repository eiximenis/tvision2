using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Core
{
    public interface IAction { }
    public interface IAction<TData> : IAction
    {
        ActionResult Invoke(TData data);
    }
}
