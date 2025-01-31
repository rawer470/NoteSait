using System;
using System.ComponentModel.DataAnnotations;

namespace NoteSait.Models;

public class FileViewModel
{
    [Required]
    public List<IFormFile> formFiles { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Message { get; set; }
    [Required]
    public string albums { get; set; }
}
