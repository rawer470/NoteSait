using System;
using System.ComponentModel.DataAnnotations;

namespace NoteSait.Models;
public class RegistrationModel
{
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string RepeatPassword { get; set; }
}
