using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId);
}