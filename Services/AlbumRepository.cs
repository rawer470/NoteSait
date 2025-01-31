using System;
using NoteSait.Data;
using NoteSait.Models.DataModel;
using NoteSait.Services.Interfaces;

namespace NoteSait.Services;

public class AlbumRepository : Repository<AlbumModel>
{
    public AlbumRepository(Context context) : base(context)
    {
    }
}
