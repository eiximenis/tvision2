using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{
    public static class FileListReducers
    {
        public static Task<FileList> FileListActions(FileList state, TvAction action)
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

                return Task.FromResult(new FileList(folder.FullName, items.ToArray()));
            }
            if (action.Name == "FETCH_INFO")
            {
                return Task.FromResult(state);
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

                return Task.FromResult(new FileList(folder.FullName, items.ToArray()));
            }

            return Task.FromResult(state);
        }
        public static Task<GlobalState> FileActions(GlobalState state, TvAction action)
        {
            if (action.Name == "BEGIN_RENAME")
            {
                return Task.FromResult(state);
            }

            return Task.FromResult(state);
        }
    }
}
