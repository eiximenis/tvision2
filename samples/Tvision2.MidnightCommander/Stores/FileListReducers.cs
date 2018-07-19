using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{
    internal static class FileListReducers
    {
        public static FileList RefreshFolder(FileList state, TvAction action)
        {
            if (action.Name == "FETCH_DIR")
            {
                var folder = action.WithData<string>().Value;
                var items = System.IO.Directory.GetFileSystemEntries(folder, "*.*");
                return new FileList(items);
            }

            return state;
        }
    }
}
