using System;

namespace Tvision2.Controls.Styles
{
    public interface IStyleBuilder
    {
        IStyleBuilder WithForegroundColor(ConsoleColor color);
        IStyleBuilder WithBackgroundColor(ConsoleColor color);
    }
}