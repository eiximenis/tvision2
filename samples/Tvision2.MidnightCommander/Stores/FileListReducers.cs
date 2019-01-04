using System.IO;
using System.Linq;
using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{
    internal static class FileListReducers
    {
        public static FileList FileListActions(FileList state, TvAction action)
        {
            if (action.Name == "FETCH_DIR")
            {
                var folderName = action.WithData<string>().Value;
                var folder = new DirectoryInfo(folderName);

                var items = folder.GetFileSystemInfos().Select(f => new FileItem
                {
                    FileAttributes = f.Attributes,
                    Name = f.Name,
                    IsDirectory = (f.Attributes & FileAttributes.Directory) != 0,
                    FullName = f.FullName
                });

                return new FileList(folder.FullName, items.ToArray());
            }
            if (action.Name == "FETCH_INFO")
            {
                return state;
            }

            if (action.Name == "FETCH_BACK")
            {
                var folderName = $"{state.CurrentFolder}{Path.DirectorySeparatorChar}..";
                var folder = new DirectoryInfo(folderName);

                var items = folder.GetFileSystemInfos().Select(f => new FileItem
                {
                    FileAttributes = f.Attributes,
                    Name = f.Name,
                    IsDirectory = (f.Attributes & FileAttributes.Directory) != 0,
                    FullName = f.FullName
                });

                return new FileList(folder.FullName, items.ToArray());
            }

            return state;
        }
        public static GlobalState FileActions(GlobalState state, TvAction action)
        {
            if (action.Name == "BEGIN_RENAME")
            {
                return state;
            }

            return state;
        }
    }
}
