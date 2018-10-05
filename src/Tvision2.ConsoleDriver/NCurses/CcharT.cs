using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.NCurses
{
    [StructLayout(LayoutKind.Sequential)]
    public struct  CcharT
    {
        public int attr;
        [MarshalAs(UnmanagedType.ByValArray)]
        public byte[] chars;
        
        public CcharT(int attr, char unicodechar)
        {
            chars = new byte[5];
            this.attr = attr;
            var bytes =  Encoding.UTF8.GetBytes(new char[] {unicodechar});
            Array.Copy(bytes, chars, bytes.Length);
            chars[bytes.Length] = 0x0;
        }
    }
}