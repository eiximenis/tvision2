using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Tvision2.ConsoleDriver.Terminfo
{
    public class TerminfoBindings
    {
        [DllImport("libtinfo.so")]
        public static extern int tgetent(IntPtr ignored, string name);
        

        [DllImport("libtinfo.so")]
        public static extern int putp([MarshalAs(UnmanagedType.LPStr)]string str);
        
        [DllImport("libtinfo.so")]
        public static extern int tigetnum (string capname);
        
        [DllImport("libtinfo.so")]
        public static extern int tigetflag(string capname);

        
        [DllImport("libtinfo.so", EntryPoint = "tigetstr")]
        public static extern IntPtr _tigetstr (string capname);
        
        [DllImport("libtinfo.so")]
        public static extern int setupterm(string name, int filddes, IntPtr errret);
 
        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern  IntPtr _tparm(string str, int p0, int p1);
        

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3, int p4);
        
        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3, int p4, int p5);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3, int p4, int p5, int p6);
        
        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7);

        [DllImport("libtinfo.so", EntryPoint = "tparm")]
        private static extern IntPtr _tparm(string str, int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8);


        
        public static string tparm(string str)
        {
            var ptr = _tparm(str);
            return Marshal.PtrToStringAnsi(ptr);
        }
        
        public static string tparm(string str, int p0)
        {
            var ptr = _tparm(str, p0);
            return Marshal.PtrToStringAnsi(ptr);
        }
        public static string tparm(string str, int p0, int p1)
        {
            var ptr = _tparm(str, p0, p1);
            return Marshal.PtrToStringAnsi(ptr);
        }
        
        public static string tigetstr(string capname)
        {
            var ptr = _tigetstr(capname);
            if (ptr.ToInt32() > 0)
            {
                return Marshal.PtrToStringAnsi(ptr);
            }

            return null;
        }

    }
    
}