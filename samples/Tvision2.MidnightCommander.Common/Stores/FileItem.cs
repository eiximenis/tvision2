using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tvision2.MidnightCommander.Stores
{
    public class FileItem
    {
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public FileAttributes FileAttributes { get; set; }
        public string FullName { get; set; }
    }
}
