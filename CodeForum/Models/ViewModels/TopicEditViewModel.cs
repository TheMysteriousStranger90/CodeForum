using System.ComponentModel.DataAnnotations;
using CodeForum.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CodeForum.Models.ViewModels;

public class TopicEditViewModel
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 3)]
    public string Title { get; set; }

    [Required] public string Content { get; set; }
}