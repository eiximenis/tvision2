using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Render;

namespace Tvision2.Controls.Styles
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

        public IStyle GetStyle(IEnumerable<string> classes)
        {
            foreach (var className in classes)
            {
                if (_styles.ContainsKey(className))
                {
                    return _styles[className];
                }
            }

            return _styles.ContainsKey("") ? _styles[""] : DefaultStyle.Instance;
        }

        public AppliedStyle BuildStyle()
        {
            var runtimeStyle = new AppliedStyle(this);
            UpdateStyle(runtimeStyle);
            return runtimeStyle;
        }

        public void UpdateStyle(AppliedStyle styleToUpdate)
        {
            var classes = styleToUpdate.Classes;
            var style = GetStyle(classes);
            styleToUpdate.BackColor = style.BackColor;
            styleToUpdate.ForeColor = style.ForeColor;
        }
    }
}
