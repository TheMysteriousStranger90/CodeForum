using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    public RatingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddRatingToTopicAsync(int topicId, Rating rating)
    {
        rating.TopicId = topicId;
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<double?> GetAverageRatingByTopicIdAsync(int topicId)
    {
        var ratings = await _context.Ratings
            .Where(r => r.TopicId == topicId)
            .ToListAsync();

        if (ratings.Any())
        {
            return ratings.Average(r => r.Score);
        }
        else
        {
            return null;
        }
    }
    
    public async Task<Rating> GetRatingByTopicAndUserAsync(int topicId, string userId)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r => r.TopicId == topicId && r.UserId == userId);
    }
}