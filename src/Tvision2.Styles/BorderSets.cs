using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tvision2.Styles
{
    public class BorderSets
    {
        private static Dictionary<BorderValue, char[]> _characters;

        public static class Entries
        {
            public const int TOPLEFT = 0;
            public const int HORIZONTAL = 1;
            public const int TOPRIGHT = 2;
            public const int VERTICAL = 3;
            public const int BOTTOMLEFT = 4;
            public const int BOTTOMRIGHT = 5;
            public const int TOPMIDDLE = 6;
            public const int BOTTOMMIDDLE = 7;
            public const int CROSS = 8;
        }

        static BorderSets()
        {
            _characters = new Dictionary<BorderValue, char[]>();
            FillCharacters();
        }

        private static void FillCharacters()
        {
            _characters.Add(BorderValue.Double(), new char[] { '\u2554', '\u2550', '\u2557', '\u2551', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c' });
            _characters.Add(BorderValue.Single(), new char[] { '\u250c', '\u2500', '\u2510', '\u2502', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Single), new char[] { '\u2552', '\u2550', '\u2555', '\u2502', '\u2558', '\u255b', '\u2567', '\u2564', '\u256a' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Single, BorderType.Double), new char[] { '\u2553', '\u2500', '\u2556', '\u2551', '\u2559', '\u255c', '\u2568', '\u2565', '\u256b' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Fill), new char[] { '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Single), new char[] { '\u250c', '\u2591', '\u2510', '\u2502', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.Double), new char[] { '\u2554', '\u2591', '\u2557', '\u2551', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.Fill), new char[] { '\u2554', '\u2550', '\u2557', '\u2591', '\u255a', '\u255d', '\u2566', '\u2569', '\u256c' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Single, BorderType.Fill), new char[] { '\u250c', '\u2500', '\u2510', '\u2591', '\u2514', '\u2518', '\u252c', '\u2534', '\u253c' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Double, BorderType.None), new char[] { '\u2550', '\u2550', '\u2550', ' ', '\u2550', '\u2550', '\u2550', '\u2550', '\u2550' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Single, BorderType.None), new char[] { '\u2500', '\u2500', '\u2500', ' ', '\u2500', '\u2500', '\u2500', '\u2500', '\u2500' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.Fill, BorderType.None), new char[] { '\u2591', '\u2591', '\u2591', ' ', '\u2591', '\u2591', '\u2591', '\u2591', '\u2591' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.None, BorderType.Double), new char[] { ' ', ' ', ' ', '\u2551', ' ', ' ', ' ', ' ', '\u2551' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.None, BorderType.Single), new char[] { ' ', ' ', ' ', '\u2502', ' ', ' ', ' ', ' ', '\u2502' });
            _characters.Add(BorderValue.FromHorizontalAndVertical(BorderType.None, BorderType.Fill), new char[] { ' ', ' ', ' ', '\u2591', ' ', ' ', ' ', ' ', '\u2591' });
        }

        public static char[] GetBorderSet(BorderValue value) => _characters.TryGetValue(value, out var characters) ? characters : _characters[BorderValue.Double()];
    }
}
