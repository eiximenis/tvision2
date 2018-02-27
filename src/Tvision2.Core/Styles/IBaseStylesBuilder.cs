using System;

namespace Tvision2.Core.Styles
{
    public interface IBaseStylesBuilder
    {
        IBaseStylesBuilder WithForegroundColor(ConsoleColor color);
        IBaseStylesBuilder WithBackgroundColor(ConsoleColor color);
    }
}