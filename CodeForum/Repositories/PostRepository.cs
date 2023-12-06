using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Post>> GetPostsByTopicIdAsync(int topicId)
    {
        return await _context.Posts
            .Where(p => p.TopicId == topicId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId)
    {
        return await _context.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetRecentPostsAsync(int numberOfPosts)
    {
        return await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Take(numberOfPosts)
            .ToListAsync();
    }
}