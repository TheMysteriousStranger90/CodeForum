﻿using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface ILikeDislikeRepository : IGenericRepository<LikeDislike>
{
    Task<IEnumerable<LikeDislike>> GetLikesDislikesByUserIdAsync(string userId);
    Task<IEnumerable<LikeDislike>> GetLikesDislikesByPostIdAsync(int postId);
}