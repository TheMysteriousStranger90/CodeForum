using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class LikeDislike
{
    [Key] public int Id { get; set; }
    public bool IsLike { get; set; }

    public int PostId { get; set; }
    [ForeignKey("PostId")] public Post Post { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }
}