using System;
using NoteSait.Data;
using NoteSait.Models;
using Microsoft.EntityFrameworkCore;
using NoteSait.Services.Interfaces;
using NoteSait.Models.DataModel;

namespace NoteSait.Services;

public class FileRepository : Repository<FileModel>, IFileRepository
{
    public Context context;

    public FileRepository(Context context) : base(context)
    {
        this.context = context;
        IncludeUserAsync();
    }

    public List<AlbumModel> GetAlbumsFromUser(string userId)
    {
        User user = context.Users.Find(userId);
        List<AlbumModel> albums = user.Albums.ToList();
        return albums;
    }
    public void IncludeUser()
    {
        context.Users.Include(x => x.Albums).ThenInclude(x=>x.Files).ToList();
    }
    public async Task IncludeUserAsync()
    {
        IncludeUser();
    }
}
