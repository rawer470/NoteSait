using System;
using NoteSait.Models;
using NoteSait.Models.DataModel;

namespace NoteSait.Services.Interfaces;

public interface IFileRepository : IRepository<FileModel>
{
    void IncludeUser();
    Task IncludeUserAsync();
    List<AlbumModel> GetAlbumsFromUser(string userId);
}
