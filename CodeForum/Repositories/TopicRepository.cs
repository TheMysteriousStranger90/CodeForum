using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Topic> GetByIdAsync(int id)
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Category)
            .Include(t => t.Posts)
            .ThenInclude(p => p.User)
            .Include(t => t.TopicTags)
            .ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Topic> GetTopicWithPostsByIdAsync(int id)
    {
        return await _context.Topics
            .Include(topic => topic.Posts)
            .FirstOrDefaultAsync(topic => topic.Id == id);
    }
    
    public async Task<IEnumerable<Topic>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Topics
            .Include(t => t.User)
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync();
    }
}