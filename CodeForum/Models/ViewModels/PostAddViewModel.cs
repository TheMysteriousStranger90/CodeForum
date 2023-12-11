using System.ComponentModel.DataAnnotations;

namespace CodeForum.Models.ViewModels;

public class PostAddViewModel
{
    [Required]
    public string Content { get; set; }
    public IFormFile ImageFile { get; set; }
    public int TopicId { get; set; }
}