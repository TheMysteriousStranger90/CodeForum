﻿using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<Favorite> GetByIdAsyncWithTopic(int id)
    {
        return await _context.Favorites
            .Include(f => f.Topic)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(string userId)
    {
        return await _context.Favorites
            .Include(f => f.Topic)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<bool> IsFavoriteAsync(int topicId, string userId)
    {
        return await _context.Favorites.AnyAsync(f => f.TopicId == topicId && f.UserId == userId);
    }
    
    public async Task<Favorite> GetByTopicIdAndUserId(int topicId, string userId)
    {
        return await _context.Favorites.FirstOrDefaultAsync(f => f.TopicId == topicId && f.UserId == userId);
    }
}