using System.Collections.Generic;

namespace Tvision2.MidnightCommander.Stores
{
    internal class FileList
    {
        public static FileList Empty { get;  }
        static FileList()
        {
            Empty = new FileList(new FileItem[0]);
        }

        private readonly FileItem[] _items;

        public IEnumerable<FileItem> Items => _items;

        public FileList(FileItem[] items)
        {
            this._items = items;
        }
    }
}
