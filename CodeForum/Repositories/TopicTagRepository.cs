using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class TopicTagRepository : GenericRepository<TopicTag>, ITopicTagRepository
{
    public TopicTagRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddTagToTopicAsync(int topicId, int tagId)
    {
        var topicTag = new TopicTag { TopicId = topicId, TagId = tagId };
        await _context.TopicTags.AddAsync(topicTag);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TopicTag>> GetTopicTagsByTopicIdAsync(int topicId)
    {
        return await _context.TopicTags
            .Where(tt => tt.TopicId == topicId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TopicTag>> GetTopicTagsByTagIdAsync(int tagId)
    {
        return await _context.TopicTags
            .Where(tt => tt.TagId == tagId)
            .ToListAsync();
    }
}