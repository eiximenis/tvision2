using System.Collections.Generic;

namespace Tvision2.MidnightCommander.Stores
{
    internal class FileList
    {
        public static FileList Empty { get;  }
        static FileList()
        {
            Empty = new FileList(new string[0]);
        }

        private readonly string[] _items;

        public IEnumerable<string> Items => _items;

        public FileList(string[] items)
        {
            this._items = items;
        }
    }
}
