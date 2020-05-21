using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.Ansi.Interop
{
    public static class Libc
    {

        // int tcgetattr(int fildes, struct termios *termios_p);
        [DllImport("libc.so.6", EntryPoint="tcgetattr")]
        public static extern int tcgetattr(int fildes, ref Termios termios_p);

    }
}
