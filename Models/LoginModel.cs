using System;
using System.ComponentModel.DataAnnotations;

namespace NoteSait.Models;
public class LoginModel
{
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public string Password { get; set; }
    
}
