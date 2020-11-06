using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using HANDLE = System.IntPtr;

namespace Tvision2.ConsoleDriver.Win32
{
    static class HandleNative
    {

        public static IntPtr INVALID_HANDLE_VALUE { get; } =  new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(HANDLE hObject);
    }
}
