using System.Security.Claims;
using CodeForum.Interfaces;
using CodeForum.Models;
using CodeForum.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

[Authorize]
public class TopicsController : Controller
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITopicTagRepository _topicTagRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileUploadService _fileUploadService;
    private readonly IWebHostEnvironment _env;

    public TopicsController(ITopicRepository topicRepository, ITopicTagRepository topicTagRepository, IRatingRepository ratingRepository, IFavoriteRepository favoriteRepository, UserManager<ApplicationUser> userManager, IFileUploadService fileUploadService,
        IWebHostEnvironment env)
    {
        _topicRepository = topicRepository;
        _topicTagRepository = topicTagRepository;
        _ratingRepository = ratingRepository;
        _favoriteRepository = favoriteRepository;
        _userManager = userManager;
        _fileUploadService = fileUploadService;
        _env = env;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (categoryId == null)
        {
            var allTopics = await _topicRepository.ListAllAsync();
            var favorites = new Dictionary<int, bool>();
            foreach (var topic in allTopics)
            {
                favorites[topic.Id] = await _favoriteRepository.IsFavoriteAsync(topic.Id, userId);
            }
            ViewData["Favorites"] = favorites;
            return View(allTopics);
        }
        else
        {
            var categoryTopics = await _topicRepository.GetByCategoryIdAsync(categoryId.Value);
            var favorites = new Dictionary<int, bool>();
            foreach (var topic in categoryTopics)
            {
                favorites[topic.Id] = await _favoriteRepository.IsFavoriteAsync(topic.Id, userId);
            }
            ViewData["Favorites"] = favorites;
            return View(categoryTopics);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }
    
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic != null)
        {
            _topicRepository.Delete(topic);
            await _topicRepository.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
    
    
    
    
    
    
    
    
    
    
    
    [HttpPost]
    public async Task<IActionResult> AddTagToTopic(TopicTagViewModel model)
    {
        var topic = await _topicRepository.GetByIdAsync(model.TopicId);
        if (topic == null)
        {
            return NotFound();
        }

        var topicTag = new TopicTag { TopicId = model.TopicId, TagId = model.TagId };
        _topicTagRepository.Add(topicTag);
        return RedirectToAction("Index", "Topics");
    }

    [HttpPost]
    public async Task<IActionResult> RateTopic(int topicId, RatingViewModel rating)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic == null)
        {
            return NotFound();
        }
    
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        var result = new Rating { TopicId = topicId, Score = rating.Score, UserId = userId};
        _ratingRepository.Add(result);
        return RedirectToAction("Index", "Topics");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Topics/FavoriteTopic/{topicId}")]
    public async Task<IActionResult> FavoriteTopic(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        var user = await _userManager.GetUserAsync(User);
        if (topic == null || user == null)
        {
            return NotFound();
        }

        var existingFavorite = await _favoriteRepository.GetByTopicIdAndUserId(topicId, user.Id);
        if (existingFavorite != null)
        {
            _favoriteRepository.Delete(existingFavorite);
        }
        else
        {
            var favorite = new Favorite { TopicId = topicId, UserId = user.Id };
            _favoriteRepository.Add(favorite);
        }

        await _favoriteRepository.SaveChangesAsync();
        return RedirectToAction("Index","Topics");
    }
}