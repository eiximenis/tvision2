using System.IO;
using System.Linq;
using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{
    internal static class FileListReducers
    {
        public static FileList RefreshFolder(FileList state, TvAction action)
        {
            if (action.Name == "FETCH_DIR")
            {
                var folderName = action.WithData<string>().Value;
                var folder = new DirectoryInfo(folderName);
                var items = folder.GetFileSystemInfos().Select(f => new FileItem
                {
                    Name = f.Name,
                    IsDirectory = (f.Attributes | FileAttributes.Directory) != 0
                });

                return new FileList(items.ToArray());
            }

            return state;
        }
    }
}
