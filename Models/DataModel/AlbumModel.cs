using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NoteSait.Data;
using NoteSait.Services;
using NoteSait.Services.Interfaces;

namespace NoteSait.Models.DataModel;

public class AlbumModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string ImagePath { get; set; }
    [Required]
    public List<FileModel> Files { get; set; } = new List<FileModel>();
    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}
