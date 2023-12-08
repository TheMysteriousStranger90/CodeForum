using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tag>> GetTagsByTopicIdAsync(int topicId)
    {
        return await _context.TopicTags
            .Where(tt => tt.TopicId == topicId)
            .Select(tt => tt.Tag)
            .ToListAsync();
    }
    
    public async Task<Tag> GetByNameAsync(string name)
    {
        return await _context.Tags
            .Where(t => t.Name == name)
            .FirstOrDefaultAsync();
    }
}