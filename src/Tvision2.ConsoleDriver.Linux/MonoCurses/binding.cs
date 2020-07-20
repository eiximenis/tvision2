
//
// binding.cs.in: Core binding for curses.
//
// Authors:
//   Miguel de Icaza (miguel.de.icaza@gmail.com)
//
// Copyright (C) 2007 Novell (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.IO;
using System.Runtime.InteropServices;
using Tvision2.ConsoleDriver.Common;

namespace Unix.Terminal {

	internal partial class Curses {

		[StructLayout (LayoutKind.Sequential)]
		internal struct MouseEvent {
			public short ID;
			public int X, Y, Z;
			public Event ButtonState;
		}

#region Screen initialization

		[DllImport ("libncursesw.so.6", EntryPoint="initscr")]
		extern static internal IntPtr real_initscr ();
		static int lines, cols;

		static Window main_window;
		private static IntPtr curses_handle, curscr_ptr, lines_ptr, cols_ptr, colors_ptr, colorpairs_ptr;

		static void FindNCurses ()
		{
            if (File.Exists("/usr/lib/libncursesw.dylib"))
                curses_handle = dlopen("libncursesw.dylib", 1);
            else
            {
                curses_handle = dlopen("libncursesw.so.6", 1);
            }
			
			if (curses_handle == IntPtr.Zero)
				throw new Exception ("Could not dlopen ncurses");

			stdscr = read_static_ptr ("stdscr");
			curscr_ptr = get_ptr ("curscr");
			lines_ptr = get_ptr ("LINES");
			cols_ptr = get_ptr ("COLS");
			colors_ptr = get_ptr("COLORS");
			colorpairs_ptr = get_ptr("COLOR_PAIRS");
		}
		
		static public Window initscr ()
		{
			FindNCurses ();
			
			main_window = new Window (real_initscr ());
			try {
				console_sharp_get_dims (out lines, out cols);
			} catch (DllNotFoundException){
				endwin ();
				Console.Error.WriteLine ("Unable to find the @MONO_CURSES@ native library\n" + 
							 "this is different than the managed mono-curses.dll\n\n" +
							 "Typically you need to install to a LD_LIBRARY_PATH directory\n" +
							 "or DYLD_LIBRARY_PATH directory or run /sbin/ldconfig");
				Environment.Exit (1);
			}
			return main_window;
		}

		public static int Lines {	
			get {
				return lines;
			}
		}

		public static int Cols {
			get {
				return cols;
			}
		}

		//
		// Returns true if the window changed since the last invocation, as a
		// side effect, the Lines and Cols properties are updated
		//
		public static bool CheckWinChange ()
		{
			int l, c;
			
			console_sharp_get_dims (out l, out c);
			if (l != lines || c != cols){
				lines = l;
				cols = c;
				return true;
			}
			return false;
		}
		
		[DllImport ("libncursesw.so.6")]
		extern static public int endwin ();

		[DllImport ("libncursesw.so.6")]
		extern static public bool isendwin ();

		//
		// Screen operations are flagged as internal, as we need to
		// catch all changes so we can update newscr, curscr, stdscr
		//
		[DllImport ("libncursesw.so.6")]
		extern static public IntPtr internal_newterm (string type, IntPtr file_outfd, IntPtr file_infd);

		[DllImport ("libncursesw.so.6")]
		extern static public IntPtr internal_set_term (IntPtr newscreen);

		[DllImport ("libncursesw.so.6")]
	        extern static internal void internal_delscreen (IntPtr sp);
#endregion

#region Input Options
		[DllImport ("libncursesw.so.6")]
		extern static public int cbreak ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public int nocbreak ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public int echo ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public int noecho ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public int halfdelay (int t);

		[DllImport ("libncursesw.so.6")]
		extern static public int raw ();

		[DllImport ("libncursesw.so.6")]
		extern static public int noraw ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public void noqiflush ();
		
		[DllImport ("libncursesw.so.6")]
		extern static public void qiflush ();

		[DllImport ("libncursesw.so.6")]
		extern static public int typeahead (IntPtr fd);

		[DllImport ("libncursesw.so.6")]
		extern static public int timeout (int delay);

        [DllImport("libncursesw.so.6")]
        extern static public int nodelay(IntPtr win, bool bf);

		//
		// Internal, as they are exposed in Window
		//
		[DllImport ("libncursesw.so.6")]
		extern static internal int wtimeout (IntPtr win, int delay);
	       
		[DllImport ("libncursesw.so.6")]
		extern static internal int notimeout (IntPtr win, bool bf);

		[DllImport ("libncursesw.so.6")]
		extern static internal int keypad (IntPtr win, bool bf);
		
		[DllImport ("libncursesw.so.6")]
		extern static internal int meta (IntPtr win, bool bf);
		
		[DllImport ("libncursesw.so.6")]
		extern static internal int intrflush (IntPtr win, bool bf);
#endregion

#region Output Options
		[DllImport ("libncursesw.so.6")]
		extern internal static int clearok (IntPtr win, bool bf);
		[DllImport ("libncursesw.so.6")]
		extern internal static int idlok (IntPtr win, bool bf);
		[DllImport ("libncursesw.so.6")]
		extern internal static void idcok (IntPtr win, bool bf);
		[DllImport ("libncursesw.so.6")]
		extern internal static void immedok (IntPtr win, bool bf);
		[DllImport ("libncursesw.so.6")]
		extern internal static int leaveok (IntPtr win, bool bf);
		[DllImport ("libncursesw.so.6")]
		extern internal static int wsetscrreg (IntPtr win, int top, int bot);
		[DllImport ("libncursesw.so.6")]
		extern internal static int scrollok (IntPtr win, bool bf);
		
		[DllImport ("libncursesw.so.6")]
		extern public static int nl();
		[DllImport ("libncursesw.so.6")]
		extern public static int nonl();
		[DllImport ("libncursesw.so.6")]
		extern public static int setscrreg (int top, int bot);
		
#endregion

#region refresh functions

		[DllImport ("libncursesw.so.6")]
		extern public static int refresh ();
		[DllImport ("libncursesw.so.6")]
		extern public static int doupdate();

		[DllImport ("libncursesw.so.6")]
		extern internal static int wrefresh (IntPtr win);
		[DllImport ("libncursesw.so.6")]
		extern internal static int redrawwin (IntPtr win);
		[DllImport ("libncursesw.so.6")]
		extern internal static int wredrawwin (IntPtr win, int beg_line, int num_lines);
		[DllImport ("libncursesw.so.6")]
		extern internal static int wnoutrefresh (IntPtr win);
#endregion

#region Output
		[DllImport ("libncursesw.so.6")]
		extern public static int move (int line, int col);

		[DllImport ("libncursesw.so.6", EntryPoint="addch")]
		extern internal static int _addch (int ch);
		
		[DllImport ("libncursesw.so.6")]
		extern public static int addstr (string s);

		[DllImport("libncursesw.so.6")]
		extern public static int mvadd_wch(int y, int x, ref CcharT character);

		static char [] r = new char [1];

		//
		// Have to wrap the native addch, as it can not
		// display unicode characters, we have to use addstr
		// for that.   but we need addch to render special ACS
		// characters
		//
		public static int addch (int ch)
		{
			if (ch < 127 || ch > 0xffff )
				return _addch (ch);
			char c = (char) ch;
			return addstr (new String (c, 1));
		}
		
		[DllImport ("libncursesw.so.6")]
		extern internal static int wmove (IntPtr win, int line, int col);

		[DllImport ("libncursesw.so.6")]
		extern internal static int waddch (IntPtr win, int ch);
#endregion

#region Attributes
		[DllImport ("libncursesw.so.6")]
		extern public static int attron (int attrs);
		[DllImport ("libncursesw.so.6")]
		extern public static int attroff (int attrs);
		[DllImport ("libncursesw.so.6")]
		extern public static int attrset (int attrs);
#endregion

#region Input
		[DllImport ("libncursesw.so.6")]
		extern public static int getch ();
		
		[DllImport ("libncursesw.so.6")]
		extern public static int get_wch (out int sequence);

		[DllImport ("libncursesw.so.6")]
		extern public static int ungetch (int ch);

		[DllImport ("libncursesw.so.6")]
		extern public static int mvgetch (int y, int x);
#endregion
		
#region Colors
		[DllImport ("libncursesw.so.6")]
		extern internal static bool has_colors ();
		public static bool HasColors => has_colors ();

		[DllImport ("libncursesw.so.6")]
		extern internal static int start_color ();
		public static int StartColor () => start_color ();

		[DllImport ("libncursesw.so.6")]
		extern internal static int init_pair (short pair, short f, short b);
		public static int InitColorPair (short pair, short foreground, short background) => init_pair (pair, foreground, background);

		[DllImport ("libncursesw.so.6")]
		extern internal static int use_default_colors ();
		public static int UseDefaultColors () => use_default_colors ();
		
		[DllImport ("libc")]
		extern internal static int setlocale (int category, string locale);
		

		public static int Colors => Marshal.ReadInt32(colors_ptr);
		public static int ColorPairs => Marshal.ReadInt32(colorpairs_ptr);
		

		[DllImport ("libncursesw.so.6")]
		extern internal static int init_color (short color, short r, short g, short b);
		public static int InitColor (short color, short r, short g, short b) => init_color (color, r,g,b);
		
		[DllImport ("libncursesw.so.6")]
		extern internal static int color_content (short color, out short r, out short g, out short b);

		public static (short red, short green, short blue) ColorContent(short color)
		{
			short red, green, blue;
			color_content(color, out red, out green, out blue);
			return (red, green, blue);
		}
		
		[DllImport ("libncursesw.so.6")]
		extern internal static bool can_change_color();

		public static bool CanChangeColor() => can_change_color();

		
#endregion
		
		[DllImport ("dl")]
		extern static IntPtr dlopen (string file, int mode);

		[DllImport ("dl")]
		extern static IntPtr dlsym (IntPtr handle, string symbol);

		static IntPtr stdscr;

		static IntPtr get_ptr (string key)
		{
			var ptr = dlsym (curses_handle, key);
			if (ptr == IntPtr.Zero)
				throw new Exception ("Could not load the key " + key);
			return ptr;
		}
		
		internal static IntPtr read_static_ptr (string key)
		{
			var ptr = get_ptr (key);
			return Marshal.ReadIntPtr (ptr);
		}

		internal static IntPtr console_sharp_get_stdscr () => stdscr;
		
		
#region Helpers
		internal static IntPtr console_sharp_get_curscr ()
		{
			return Marshal.ReadIntPtr (curscr_ptr);
		}

		internal static void console_sharp_get_dims (out int lines, out int cols)
		{
			lines = Marshal.ReadInt32 (lines_ptr);
			cols = Marshal.ReadInt32 (cols_ptr);
		}
		

		[DllImport ("libncursesw.so.6", EntryPoint="mousemask")]
		extern static IntPtr call_mousemask (IntPtr newmask, out IntPtr oldmask);
		
		public static Event mousemask (Event newmask, out Event oldmask)
		{
			IntPtr e;
			var ret = (Event) call_mousemask ((IntPtr) newmask, out e);
			oldmask = (Event) e;
			return ret;
		}

		[DllImport ("libncursesw.so.6")]
		public extern static uint getmouse (out MouseEvent ev);

		[DllImport ("libncursesw.so.6")]
		public extern static uint ungetmouse (ref MouseEvent ev);

		[DllImport ("libncursesw.so.6")]
		public extern static int mouseinterval (int interval);
#endregion

		// We encode ESC + char (what Alt-char generates) as 0x2000 + char
		public const int KeyAlt = 0x2000;

		static public int IsAlt (int key)
		{
			if ((key & KeyAlt) != 0)
				return key & ~KeyAlt;
			return 0;
		}
	}
}
