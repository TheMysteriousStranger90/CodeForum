using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class Favorite
{
    [Key] public int Id { get; set; }

    public int TopicId { get; set; }
    [ForeignKey("TopicId")] public Topic Topic { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }
}