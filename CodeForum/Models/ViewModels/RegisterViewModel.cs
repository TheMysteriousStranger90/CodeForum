using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CodeForum.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First Name is required"), NotNull]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "The First Name value cannot exceed 70 characters. ")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required"), NotNull]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
    [StringLength(70, MinimumLength = 2, ErrorMessage = "The Last Name value cannot exceed 70 characters. ")]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required] [Display(Name = "Email")] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string PasswordConfirm { get; set; }
}