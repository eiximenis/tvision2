﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Events;

namespace Tvision2.Engine.Console
{
    public interface IConsoleDriver
    {
        void WriteCharacterAt(int x, int y, char character);
        void WriteCharacterAt(int x, int y, char character, int pairIdx);
        void SetCursorAt(int x, int y);
        TvConsoleEvents ReadEvents();
        void Init();
        (int rows, int cols) GetConsoleWindowSize();
    }
}
