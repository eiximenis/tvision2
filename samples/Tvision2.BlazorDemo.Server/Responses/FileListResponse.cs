using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tvision2.BlazorDemo.Server.Responses
{
    class FileListResponse
    {
        public IEnumerable<FileResponse> Files { get; private set; }
        public string FolderFullName { get; internal set; }

        public void SetFiles(IEnumerable<FileResponse> data)
        {
            Files = data.ToList();
        }
    }

    class FileResponse
    {
        public string Name { get; internal set; }
        public string FullName { get; internal set; }
        public FileAttributes Attributes { get; set; }
    }
}
