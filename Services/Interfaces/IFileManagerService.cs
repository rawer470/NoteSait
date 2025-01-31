using System;
using NoteSait.Models;
using NoteSait.Models.DataModel;

namespace NoteSait.Services.Interfaces;

public interface IFileManagerService
{
    bool AddFile(FileViewModel fileView);
    Task AddFileToDbAsync(FileModel fileModel);
    bool DeleteFile(string id);
    bool DeleteFileByPath(string path);
    FileModel GetFileById(string id);
    DownloadedFile GetFileBytesById(string id);
    List<AlbumModel> GetAlbumsFromUser();
}
