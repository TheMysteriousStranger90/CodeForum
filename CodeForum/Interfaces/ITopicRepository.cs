using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface ITopicRepository : IGenericRepository<Topic>
{
    Task<Topic> GetTopicWithPostsByIdAsync(int id);
}