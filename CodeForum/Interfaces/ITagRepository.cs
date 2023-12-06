using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsByTopicIdAsync(int topicId);
}