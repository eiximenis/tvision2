using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Core.Styles
{
    public class StyleSheet : IStyleSheet
    {

        private static StyleSheet _default;

        private readonly IDictionary<string, IStyle> _styles;

        static StyleSheet() => _default = new StyleSheet();

        private StyleSheet() : this(new Dictionary<string, IStyle>()) { }

        public static IStyleSheet Default => _default;

        public StyleSheet(IDictionary<string, IStyle> styles)
        {
            _styles = styles;
        }

        public void AddClassStyle(string name, IStyle style)
        {
            if (!_styles.ContainsKey(name))
            {
                _styles.Add(name, style);
            }
            else
            {
                _styles[name] = style;
            }
        }

        public void RemoveClassStyle(string name)
        {
            if (_styles.ContainsKey(name))
            {
                _styles.Remove(name);
            }
        }


        public ConsoleColor GetForeColor(IEnumerable<string> classes)
        {
            foreach (var className in classes)
            {
                if (_styles.ContainsKey(className))
                {
                    return _styles[className].ForeColor;
                }
            }

            return DefaultStyle.Instance.ForeColor;
        }

        public ConsoleColor GetBackColor(IEnumerable<string> classes)
        {
            foreach (var className in classes)
            {
                if (_styles.ContainsKey(className))
                {
                    return _styles[className].BackColor;
                }
            }

            return DefaultStyle.Instance.BackColor;
        }
    }
}
