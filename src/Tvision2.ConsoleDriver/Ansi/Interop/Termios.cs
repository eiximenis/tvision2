using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.Ansi.Interop
{

    using tcflag_t = UInt32;
    using cc_t = Byte;
    using speed_t = UInt32;

    // /usr/include/x86_64-linux-gnu/bits/termios.h
    [StructLayout(LayoutKind.Sequential)]
    public struct Termios
    {
        public tcflag_t c_iflag;       /* input mode flags */
        public tcflag_t c_oflag;       /* output mode flags */
        public tcflag_t c_cflag;       /* control mode flags */
        public tcflag_t c_lflag;       /* local mode flags */
        public cc_t c_line;            /* line discipline */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public cc_t[] c_cc;            /* control characters */
        public speed_t c_ispeed;       /* input speed */
        public speed_t c_ospeed;       /* output speed */
    }
}
