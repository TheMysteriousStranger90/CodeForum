using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class Report
{
    [Key] public int Id { get; set; }
    public string Content { get; set; }
    public DateTime ReportedAt { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")] public ApplicationUser User { get; set; }

    public int? TopicId { get; set; }
    [ForeignKey("TopicId")] public Topic Topic { get; set; }

    public int? PostId { get; set; }
    [ForeignKey("PostId")] public Post Post { get; set; }
}