using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<IEnumerable<Post>> GetPostsByTopicIdAsync(int topicId);
    Task<IEnumerable<Post>> GetPostsByUserIdAsync(string userId);
    Task<IEnumerable<Post>> GetRecentPostsAsync(int numberOfPosts);
}