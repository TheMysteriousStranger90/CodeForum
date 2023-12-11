namespace CodeForum.Models.ViewModels;

public class PostEditViewModel
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int TopicId { get; set; }
}