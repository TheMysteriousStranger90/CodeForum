using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class Notification
{
    [Key] public int Id { get; set; }
    public string Content { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }
}
