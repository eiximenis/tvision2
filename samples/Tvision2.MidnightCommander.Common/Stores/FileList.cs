using System.Collections.Generic;

namespace Tvision2.MidnightCommander.Stores
{
    public class FileList
    {

        public string CurrentFolder { get; }

        public static FileList Empty { get;  }
        static FileList()
        {
            Empty = new FileList("", new FileItem[0]);
        }

        private readonly FileItem[] _items;

        public IEnumerable<FileItem> Items => _items;

        public FileList(string name, FileItem[] items)
        {
            CurrentFolder = name;
            this._items = items;
        }
    }
}
