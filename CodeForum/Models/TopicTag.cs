using System.ComponentModel.DataAnnotations.Schema;

namespace CodeForum.Models;

public class TopicTag
{
    public int TopicId { get; set; }
    [ForeignKey("TopicId")] public Topic Topic { get; set; }

    public int TagId { get; set; }
    [ForeignKey("TagId")] public Tag Tag { get; set; }
}