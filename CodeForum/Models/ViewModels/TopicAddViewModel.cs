namespace CodeForum.Models.ViewModels;

public class TopicAddViewModel
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; }
    public IFormFile ImageFile { get; set; }
}