﻿using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Topic> GetTopicWithPostsByIdAsync(int id)
    {
        return await _context.Topics
            .Include(topic => topic.Posts)
            .FirstOrDefaultAsync(topic => topic.Id == id);
    }
}