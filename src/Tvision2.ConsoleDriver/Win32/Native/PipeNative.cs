using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.Win32
{

    [StructLayout(LayoutKind.Sequential)]
    struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }

    static class PipeNative
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreatePipe([Out] out IntPtr hReadPipe, [Out] out IntPtr hWritePipe, ref SECURITY_ATTRIBUTES lpPipeAttributes, uint nSize);
    }
}
