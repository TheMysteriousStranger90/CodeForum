using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task AddRatingToTopicAsync(int topicId, Rating rating);
    Task<double?> GetAverageRatingByTopicIdAsync(int topicId);
}