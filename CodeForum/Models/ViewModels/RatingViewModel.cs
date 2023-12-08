using System.ComponentModel.DataAnnotations;

namespace CodeForum.Models.ViewModels;

public class RatingViewModel
{
    public int TopicId { get; set; }

    [Range(1, 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Score { get; set; }

    public string UserId { get; set; }
}