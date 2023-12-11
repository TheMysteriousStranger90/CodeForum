using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();
    }
    
    public async Task CreatePostNotificationAsync(Post post)
    {
        var topic = await _context.Topics.FindAsync(post.TopicId);
        var notification = new Notification
        {
            UserId = topic.UserId,
            Content = $"Your topic has been commented on by {post.UserId}",
            IsRead = false,
            CreatedAt = DateTime.Now
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }
}