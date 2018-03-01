using System;

namespace Tvision2.Core.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder WithForegroundColor(ConsoleColor color);
        IStyleBuilder WithBackgroundColor(ConsoleColor color);
    }
}