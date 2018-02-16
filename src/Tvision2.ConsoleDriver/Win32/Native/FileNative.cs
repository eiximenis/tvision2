using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using DWORD = System.UInt32;
using HANDLE = System.IntPtr;

namespace Tvision2.ConsoleDriver.Win32
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    struct OVERLAPPED
    {
        [FieldOffset(0)]
        public uint Internal;

        [FieldOffset(4)]
        public uint InternalHigh;

        [FieldOffset(8)]
        public uint Offset;

        [FieldOffset(12)]
        public uint OffsetHigh;

        [FieldOffset(8)]
        public IntPtr Pointer;

        [FieldOffset(16)]
        public IntPtr hEvent;
    }

    enum FILE_TYPE : DWORD
    {
        FILE_TYPE_CHAR = 0x0002,
        FILE_TYPE_DISK = 0x0001,
        FILE_TYPE_PIPE = 0x0003,
        FILE_TYPE_REMOTE = 0x8000,
        FILE_TYPE_UNKNOWN = 0x0000
    }

    static class FileNative
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern FILE_TYPE GetFileType(HANDLE hFile);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(HANDLE hFile, byte[] lpBuffer, DWORD nNumberOfBytesToWrite, [Out] out DWORD lpNumberOfBytesWritten, [Out] IntPtr lpOverlapped);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(HANDLE hFile, byte[] lpBuffer, DWORD nNumberOfBytesToWrite, [Out] out DWORD lpNumberOfBytesWritten, [Out] out OVERLAPPED lpOverlapped);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(HANDLE hFile, byte[] lpBuffer, DWORD nNumberOfBytesToRead, [Out] out DWORD lpNumberOfBytesRead, [Out] out OVERLAPPED lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(HANDLE hFile, byte[] lpBuffer, DWORD nNumberOfBytesToRead, [Out] out DWORD lpNumberOfBytesRead, [Out] IntPtr lpOverlapped);

    }
}
