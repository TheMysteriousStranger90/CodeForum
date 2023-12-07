using CodeForum.Validations;

namespace CodeForum.Models.ViewModels;

public class EditUserViewModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }

    [FileTypeValidation(".jpg", ".jpeg", ".png")]
    public IFormFile ProfilePicture { get; set; }
    public string CurrentProfilePicture { get; set; }
}