using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Styles
{
    public static class TuiEngineBuilderExtensions_StylesMc
    {
        public static ISkinManagerBuilder AddMcStyles (this ISkinManagerBuilder builder)
        {
            builder.AddSkin("Mc", sb =>
            {
                sb.AddStyleSheet("tvbutton", shb =>
                {
                    shb.AddClass("", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.White);
                        style.WithForegroundColor(ConsoleColor.Black);
                    });
                    shb.AddClass("focused", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.Blue);
                        style.WithForegroundColor(ConsoleColor.Black);
                    });

                });

                sb.AddStyleSheet("tvlist", shb =>
                {
                    shb.AddClass("", style =>
                    {
                        style.WithForegroundColor(ConsoleColor.White);
                        style.WithBackgroundColor(ConsoleColor.Blue);
                    });
                });

                sb.AddStyleSheet("tvmenubar", shb =>
                {
                    shb.AddClass("", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.DarkBlue);
                        style.WithForegroundColor(ConsoleColor.White);
                    });
                    shb.AddClass("focused", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.Black);
                        style.WithForegroundColor(ConsoleColor.White);
                    });

                });

                sb.AddStyleSheet("tvtextbox", shb =>
                {
                    shb.AddClass("", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.White);
                        style.WithForegroundColor(ConsoleColor.Black);
                    });
                    shb.AddClass("focused", style =>
                    {
                        style.WithBackgroundColor(ConsoleColor.Blue);
                        style.WithForegroundColor(ConsoleColor.Black);
                    });
                });
            });

            return builder;
        }
    }
}
