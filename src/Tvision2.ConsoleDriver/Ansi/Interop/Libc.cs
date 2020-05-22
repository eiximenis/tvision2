using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static Tvision2.ConsoleDriver.Ansi.Interop.Types;

namespace Tvision2.ConsoleDriver.Ansi.Interop
{
    
    using nfds_t = UInt64;
    public static class Libc
    {


        // int tcgetattr(int fildes, struct termios *termios_p);
        [DllImport("libc.so.6", EntryPoint="tcgetattr")]
        public static extern int tcgetattr(int fildes, ref termios termios_p);
        
        [DllImport("libc.so.6", EntryPoint="tcsetattr")]
        public static extern int tcsetattr(int fildes, int optional_actions, ref termios termios_p);

        [DllImport("libc.so.6", EntryPoint="cfmakeraw")]
        public static extern int cfmakeraw(ref termios termios_p);

        [DllImport("libc.so.6", EntryPoint = "read")]
        private static extern int _read(int fd,  ref byte buf, int count);
        
        
        [DllImport("libc.so.6", EntryPoint = "__fdset")]
        private static extern int __fdset(int fd,  ref fd_set p);

        
        [DllImport("libc.so.6", EntryPoint = "select")]
        private static extern int select(int fd, ref fd_set readfds, IntPtr writefds, IntPtr exceptfds, ref  timeval timeout);

        
        [DllImport("libc.so.6", EntryPoint = "poll")]
        private static extern int __poll (ref pollfd __fds, nfds_t __nfds, int __timeout);


        public static bool poll()
        {
            var pfd = new pollfd();
            pfd.fd = 0;
            pfd.events = Constants.POLLIN;
            var evtsOnFd = __poll(ref pfd, 1, 1);
            return evtsOnFd > 0 && ((pfd.revents & Constants.POLLIN) == Constants.POLLIN);
        }

        public static int read()
        {
            if (poll())
            {
                byte buf = 0;
                var retval =_read(0, ref buf, 1);
                return buf;
            }

            return -1;
        }

    }
}
