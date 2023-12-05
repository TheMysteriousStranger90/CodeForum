using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class Post
{
    [Key] public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; }
    [NotMapped] public IFormFile ImageFile { get; set; }

    public int TopicId { get; set; }
    [ForeignKey("TopicId")] public Topic Topic { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }

    public ICollection<LikeDislike> LikesDislikes { get; set; }
}