using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoteSait.Models.DataModel;

namespace NoteSait.Models;
public enum StateExc
{
    OK,
    FileNotFound,
}
public class FileModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Message { get; set; }
    public string Path { get; set; }
    [NotMapped]
    public StateExc state { get; set; }
    public string? AlbumId { get; set; }
    [ForeignKey("AlbumId")]
    public AlbumModel Album { get; set; }
}
