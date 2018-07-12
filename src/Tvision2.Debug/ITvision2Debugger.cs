using Tvision2.Core.Engine;

namespace Tvision2.Debug
{
    public interface ITvision2Debugger
    {
        void AttachTo(IComponentTree uI);
    }
}