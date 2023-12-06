using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace CodeForum.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    [AllowNull] public string Bio { get; set; }
    [AllowNull] public string ProfilePicture { get; set; }
    public ICollection<Topic> Topics { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<LikeDislike> LikesDislikes { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<Report> Reports { get; set; }
}