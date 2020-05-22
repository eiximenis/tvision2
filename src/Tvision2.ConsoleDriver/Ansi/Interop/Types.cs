using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.Ansi.Interop
{

    using tcflag_t = UInt32;
    using cc_t = Byte;
    using speed_t = UInt32;
    using __time_t = UInt64;
    using __suseconds_t = UInt64;

    public static  class Constants
    {
        public const int TCSANOW = 0;
        public const int POLLIN = 0x001;
    }

    public static class Types
    {
	
        
        // /usr/include/x86_64-linux-gnu/bits/termios.h
        [StructLayout(LayoutKind.Sequential)]
        public struct termios
        {
            public tcflag_t c_iflag; /* input mode flags */
            public tcflag_t c_oflag; /* output mode flags */
            public tcflag_t c_cflag; /* control mode flags */
            public tcflag_t c_lflag; /* local mode flags */
            public cc_t c_line; /* line discipline */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public cc_t[] c_cc; /* control characters */

            public speed_t c_ispeed; /* input speed */
            public speed_t c_ospeed; /* output speed */
        }


        // usr/include/x86_64-linux-gnu/bits/types/struct_timeval.h
        [StructLayout(LayoutKind.Sequential)]
        public struct timeval
        {
            public __time_t tv_sec; /* Seconds.  */
            public __suseconds_t tv_usec; /* Microseconds.  */

        };
        
        [StructLayout(LayoutKind.Sequential)]
        public struct pollfd
        {
            public int fd;                        /* File descriptor to poll.  */
            public short events;                  /* Types of events poller cares about.  */
            public short revents;                 /* Types of events that actually occurred.  */
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct fd_set
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public IntPtr[] fds_bits;
        }
    }



}
