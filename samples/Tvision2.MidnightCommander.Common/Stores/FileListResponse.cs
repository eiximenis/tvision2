using System.Collections.Generic;
using System.IO;

namespace Tvision2.MidnightCommander.Stores
{
    internal class FileListResponse
    {
        public IEnumerable<FileResponse> Files { get; set; }
        public string FolderFullName { get; set; }
    }

    public class FileResponse
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public FileAttributes Attributes { get; set; }
    }
}