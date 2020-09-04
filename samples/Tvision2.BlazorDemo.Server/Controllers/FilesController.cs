using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tvision2.BlazorDemo.Server.Responses;

namespace Tvision2.BlazorDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get(string path)
        {
            var folder = new DirectoryInfo(path);


            var response = new FileListResponse();


            var data = folder.GetFileSystemInfos().Select(f => new FileResponse
            {
                Attributes = f.Attributes,
                Name = f.Name,
                FullName = f.FullName
            });
            response.SetFiles(data);
            response.FolderFullName = folder.FullName;
            return Ok(response);
        }
    }
}
