using System.Security.Claims;
using CodeForum.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

[Authorize]
public class LikeDislikeController : Controller
{
    private readonly ILikeDislikeRepository _likeDislikeRepository;

    public LikeDislikeController(ILikeDislikeRepository likeDislikeRepository)
    {
        _likeDislikeRepository = likeDislikeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var likesDislikes = await _likeDislikeRepository.GetLikesDislikesByUserIdAsync(userId);

        return View(likesDislikes);
    }
}