using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetCategoriesByTopicIdAsync(int topicId)
    {
        return await _context.Categories
            .Where(c => c.Topics.Any(t => t.Id == topicId))
            .ToListAsync();
    }
}