using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId);
    Task CreatePostNotificationAsync(Post post);
}