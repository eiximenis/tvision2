using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tvision2.ConsoleDriver.Linux.NCurses
{
    public class NCursesResolver
    {

        private readonly static bool _isLinux;
        private readonly static bool _isMac;
        
        private const string NCURSES_6_LINUX ="libncursesw.so.6";
        private const string NCURSES_5_LINUX ="libncursesw.so.5";
        private const string NCURSES_MAC = "curses";
        
        static NCursesResolver()
        {
            _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            _isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }
        
        
        public static IntPtr ResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var libToUse = libraryName;
            if (libraryName == "libcoreclr.so") return (IntPtr)(-1);
            
            else if (libraryName == "ncurses")
            {
                if (_isLinux)
                {
                    return ResolveNCursesOnLinux();
                }
                else if (_isMac)
                {
                    return ResolveNCursesOnMac();
                }
                return IntPtr.Zero;
            }
            return NativeLibrary.TryLoad(libToUse, out var handle) ? handle : IntPtr.Zero;
        }

        private static IntPtr ResolveNCursesOnMac()
        {
            return NativeLibrary.TryLoad(NCURSES_MAC, out var hmac) ? hmac : IntPtr.Zero;
        }

        private static IntPtr ResolveNCursesOnLinux()
        {
            if (NativeLibrary.TryLoad(NCURSES_6_LINUX, out var handle6))
            {
                return handle6;
            }
            return NativeLibrary.TryLoad(NCURSES_5_LINUX, out var handle5) ? handle5 : IntPtr.Zero;
        }
    }
}