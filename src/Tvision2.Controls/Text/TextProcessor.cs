using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Controls.Extensions;

namespace Tvision2.Controls.Text
{
    public class TextProcessor
    {
        private readonly StringBuilder _sb;

        public TextProcessor()
        {
            _sb = new StringBuilder();
        }


        public override string ToString() => _sb.ToString();

        public void AppendChar(char value) => InsertChar(value, -1);

        public void InsertChar(char value, int index)
        {
            if (index > _sb.Length)
            {
                index = _sb.Length;
            }

            _sb.Insert(index, value);
        }


        private void InsertKey(ConsoleKey key, int position)
        {
            switch (key)
            {
                case ConsoleKey.Backspace:
                    RemoveChars(position - 1);
                    break;
                case ConsoleKey.Delete:
                    RemoveChars(position);
                    break;
            }
        }

        public void RemoveChars(int offset, int count = 1)
        {
            if (offset < 0)
            {
                return;
            }

           if (offset >= _sb.Length)
            {
                offset = _sb.Length;
            }
           if (count > _sb.Length - offset)
            {
                count = _sb.Length - offset;
            }

            _sb.Remove(offset, count);
        }
    }
}
