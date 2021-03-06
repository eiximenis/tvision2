﻿using System;

namespace Tvision2.Controls.List
{
    public class ListStateView<T>
    {
        private readonly ListState<T> _source;
        private int _from;
        private int _to;

        public ListStateView(ListState<T> source)
        {
            _source = source;
            Reload();
        }

        public T this[int idx] => _source[idx + _from];

        public void Adjust(int rows)
        {
            To = From + rows - 1;
        }

        public void ScrollDown(int lines)
        {
            To += lines;
            From += lines;
        }

        public void ScrollUp(int lines)
        {
            To -= lines;
            From -= lines;
        }



        public int NumItems => _to - _from + 1;

        public int From
        {
            get => _from;
            set
            {
                _from = value;
                if (_from < 0)
                {
                    _from = 0;
                }
            }
        }


        public int To
        {
            get => _to;
            set
            {
                _to = value;
                if (_to > _source.Count - 1)
                {
                    _to = _source.Count - 1;
                }
            }
        }

        public void Reload()
        {
            _from = 0;
            if (_source.Count > 0)
            {
                _to = _source.Count - 1;
            }
            else
            {
                _to = -1;
            }
        }
    }
}
