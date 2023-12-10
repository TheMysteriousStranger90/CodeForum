using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

[Authorize]
public class FavoritesController : Controller
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoritesController(IFavoriteRepository favoriteRepository, UserManager<ApplicationUser> userManager)
    {
        _favoriteRepository = favoriteRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var favorites = await _favoriteRepository.GetFavoritesByUserIdAsync(user.Id);
        return View(favorites);
    }
}