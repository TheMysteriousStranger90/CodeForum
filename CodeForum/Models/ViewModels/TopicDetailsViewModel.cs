namespace CodeForum.Models.ViewModels;

public class TopicDetailsViewModel
{
    public Topic Topic { get; set; }
    public bool IsFavorite { get; set; }
    public double AverageRating { get; set; }
}