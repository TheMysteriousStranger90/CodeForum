using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class Topic
{
    [Key] public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Image { get; set; }
    [NotMapped] public IFormFile ImageFile { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }
    
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category Category { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<TopicTag> TopicTags { get; set; }
}