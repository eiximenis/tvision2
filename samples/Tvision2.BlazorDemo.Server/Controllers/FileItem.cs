using System.IO;

namespace Tvision2.BlazorDemo.Server.Controllers
{
    internal class FileItem
    {
        public FileAttributes FileAttributes { get; set; }
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public string FullName { get; set; }
    }
}