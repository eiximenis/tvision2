using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Statex;

namespace Tvision2.MidnightCommander.Stores
{
    public class FileListStore : TvStore<FileList>
    {
        public FileListStore(FileList state) : base(state)
        {   
        }
    }
}
