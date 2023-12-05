using System.ComponentModel.DataAnnotations;

namespace CodeForum.Models;

public class Category
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Topic> Topics { get; set; }
}