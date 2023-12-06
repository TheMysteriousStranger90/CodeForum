﻿using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class LikeDislikeRepository : GenericRepository<LikeDislike>, ILikeDislikeRepository
{
    public LikeDislikeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LikeDislike>> GetLikesDislikesByUserIdAsync(string userId)
    {
        return await _context.LikesDislikes
            .Where(ld => ld.UserId == userId)
            .ToListAsync();
    }
}