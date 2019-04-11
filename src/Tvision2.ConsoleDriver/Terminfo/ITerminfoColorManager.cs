using System.ComponentModel;
using Tvision2.Core.Colors;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public interface ITerminfoColorManager : IColorManager
    {
        void SetAttributes(CharacterAttribute attributes);
        void Init();
    }
}