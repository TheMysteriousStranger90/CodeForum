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
    
    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var favorite = await _favoriteRepository.GetByIdAsync(id);
        if (favorite == null || favorite.UserId != user.Id) return NotFound();

        _favoriteRepository.Delete(favorite);
        await _favoriteRepository.SaveChangesAsync();

        return Json(new { success = true });
    }
}