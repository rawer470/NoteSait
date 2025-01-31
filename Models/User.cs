using System;
using Microsoft.AspNetCore.Identity;
using NoteSait.Models;
using NoteSait.Models.DataModel;
namespace NoteSait.Models;

public class User : IdentityUser
{
    public List<AlbumModel> Albums { get; set; }
}
