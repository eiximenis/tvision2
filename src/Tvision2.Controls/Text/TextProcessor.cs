using System;
using System.Text;

namespace Tvision2.Controls.Text
{
    public class TextProcessor
    {
        private readonly StringBuilder _sb;

        public bool ReplaceMode { get; private set; }
        public int CurentPos { get; private set; }

        public TextProcessor()
        {
            _sb = new StringBuilder();
            CurentPos = 0;
            ReplaceMode = false;
        }


        public override string ToString() => _sb.ToString();

        internal void ProcessKey(ConsoleKeyInfo consoleKeyInfo)
        {
            var key = consoleKeyInfo.Key;
            var special = ProcessSpecialKey(key);
            if (!special && consoleKeyInfo.KeyChar != '\0') 
            {
                InsertChar(consoleKeyInfo.KeyChar);
            }
        }

        private bool ProcessSpecialKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Backspace:
                    RemoveChars(CurentPos - 1);
                    return true;
                case ConsoleKey.Delete:
                    RemoveChars(CurentPos);
                    return true;
                case ConsoleKey.LeftArrow:
                    DecreasePosition();
                    return true;
                case ConsoleKey.RightArrow:
                    IncreasePosition();
                    return true;
                case ConsoleKey.End:
                    GoToEnd();
                    return true;
                case ConsoleKey.Home:
                    GoToBegin();
                    return true;
                case ConsoleKey.Insert:
                    ReplaceMode = !ReplaceMode;
                    return true;
            }

            return false;
        }

        private void GoToEnd()
        {
            CurentPos = _sb.Length;
        }

        private void GoToBegin()
        {
            CurentPos = 0;
        }

        private void InsertChar(char value)
        {
            if (ReplaceMode && CurentPos < _sb.Length)
            {
                _sb[CurentPos] = value;
            }
            else
            {
                _sb.Insert(CurentPos, value);
            }
            IncreasePosition();
        }
        private void DecreasePosition(int count = 1)
        {
            CurentPos -= count;
            if (CurentPos < 0) CurentPos = 0;
        }

        private void IncreasePosition(int count = 1)
        {
            CurentPos += count;
            if (CurentPos > _sb.Length) CurentPos = _sb.Length;
        }


        public void RemoveChars(int pos, int count = 1)
        {
            if (pos < 0)
            {
                return;
            }

            if (pos >= _sb.Length)
            {
                pos = _sb.Length;
            }
            if (count > _sb.Length - pos)
            {
                count = _sb.Length - pos;
            }

            _sb.Remove(pos, count);
            DecreasePosition(count);
        }
    }
}
