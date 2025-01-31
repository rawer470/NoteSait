using System;
using System.ComponentModel.DataAnnotations;

namespace NoteSait.Models.ViewModel;

public class AlbumViewModel
{
    [Required]
    public IFormFile formFiles { get; set; }
    [Required]
    public string Name { get; set; }
}
