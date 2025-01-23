using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<Favorite> GetByIdAsyncWithTopic(int id);
    Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId);
    Task<bool> IsFavoriteAsync(int topicId, string userId);
    Task<Favorite> GetByTopicIdAndUserId(int topicId, string userId);
}