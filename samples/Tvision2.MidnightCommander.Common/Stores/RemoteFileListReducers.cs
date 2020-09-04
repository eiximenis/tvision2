using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tvision2.Statex;
using System.Text.Json;
using System.Linq;
using System.IO;

namespace Tvision2.MidnightCommander.Stores
{
    public static class RemoteFileListReducers
    {
        public static string RemoteUrl { get; set; }


        public static async Task<FileList> FileListActions(FileList state, TvAction action)
        {
            if (action.Name == "FETCH_DIR")
            {
                var folderName = action.WithData<string>().Value;

                var client = new HttpClient();
                var response = await client.GetAsync(RemoteUrl + "?path=" + folderName);

                if (!response.IsSuccessStatusCode)
                {
                    return new FileList(state.CurrentFolder, new FileItem[0]);
                }

                var jsonStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FileListResponse>(jsonStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var items = result.Files.Select(f => new FileItem
                {
                    FileAttributes = f.Attributes,
                    Name = f.Name,
                    IsDirectory = (f.Attributes & FileAttributes.Directory) != 0,
                    FullName = f.FullName
                });

                return new FileList(result.FolderFullName, items.ToArray());
            }
            if (action.Name == "FETCH_INFO")
            {
                return state;
            }

            if (action.Name == "FETCH_BACK")
            {
                var folderName = $"{state.CurrentFolder}{Path.DirectorySeparatorChar}..";
                var client = new HttpClient();
                var response = await client.GetAsync(RemoteUrl + "?path=" + folderName);

                if (!response.IsSuccessStatusCode)
                {
                    return new FileList(state.CurrentFolder, new FileItem[0]);
                }

                var jsonStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FileListResponse>(jsonStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var items = result.Files.Select(f => new FileItem
                {
                    FileAttributes = f.Attributes,
                    Name = f.Name,
                    IsDirectory = (f.Attributes & FileAttributes.Directory) != 0,
                    FullName = f.FullName
                });

                return new FileList(result.FolderFullName, items.ToArray());
            }

            return state;
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
