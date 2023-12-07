using System.ComponentModel.DataAnnotations;

namespace CodeForum.Models.ViewModels;

public class CategoryAddViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
    public string Name { get; set; }
}
