using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface ITopicTagRepository : IGenericRepository<TopicTag>
{
    Task AddTagToTopicAsync(int topicId, int tagId);
    Task<IEnumerable<TopicTag>> GetTopicTagsByTopicIdAsync(int topicId);
    Task<IEnumerable<TopicTag>> GetTopicTagsByTagIdAsync(int tagId);
}