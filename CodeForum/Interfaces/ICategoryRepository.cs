using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesByTopicIdAsync(int topicId);
}