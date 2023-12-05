using System.ComponentModel.DataAnnotations;

namespace CodeForum.Models;

public class Tag
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TopicTag> TopicTags { get; set; }
}
