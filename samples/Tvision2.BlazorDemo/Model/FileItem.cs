using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tvision2.BlazorDemo.Model
{
    public class FileItem
    {
        public string Name { get; set; }
        public bool IsDirectory { get; set; }

        public FileItem()
        {
            Name = Guid.NewGuid().ToString();
        }
    }
}
