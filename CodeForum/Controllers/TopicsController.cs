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
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ITopicTagRepository _topicTagRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileUploadService _fileUploadService;
    private readonly IWebHostEnvironment _env;

    public TopicsController(ITopicRepository topicRepository, ITopicTagRepository topicTagRepository,
        IRatingRepository ratingRepository, IFavoriteRepository favoriteRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository,
        UserManager<ApplicationUser> userManager, IFileUploadService fileUploadService,
        IWebHostEnvironment env)
    {
        _categoryRepository = categoryRepository;
        _topicRepository = topicRepository;
        _topicTagRepository = topicTagRepository;
        _ratingRepository = ratingRepository;
        _favoriteRepository = favoriteRepository;
        _tagRepository = tagRepository;
        _userManager = userManager;
        _fileUploadService = fileUploadService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        IEnumerable<Topic> topics;
        if (categoryId == null)
        {
            topics = await _topicRepository.ListAllAsync();
        }
        else
        {
            topics = await _topicRepository.GetByCategoryIdAsync(categoryId.Value);
            ViewBag.CategoryId = categoryId.Value; 
        }

        var favorites = new Dictionary<int, bool>();
        var ratings = new Dictionary<int, int>();
        foreach (var topic in topics)
        {
            favorites[topic.Id] = await _favoriteRepository.IsFavoriteAsync(topic.Id, userId);
            var averageRating = await _ratingRepository.GetAverageRatingByTopicIdAsync(topic.Id);
            ratings[topic.Id] = averageRating.HasValue ? (int)averageRating.Value : 0;
        }
        ViewData["Favorites"] = favorites;
        ViewData["Ratings"] = ratings;
        
        return View(topics);
    }
    
    [HttpGet]
    public IActionResult Create(int categoryId)
    {
        var model = new TopicAddViewModel { CategoryId = categoryId };
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(TopicAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = await _categoryRepository.GetByIdAsync(model.CategoryId);
        if (category == null)
        {
            ModelState.AddModelError("", "The selected category does not exist.");
            return View(model);
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        var topic = new Topic
        {
            Title = model.Title,
            Content = model.Content,
            CategoryId = model.CategoryId,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
    
        if (model.ImageFile != null)
        {
            var fileName = Path.GetFileName(model.ImageFile.FileName);
            var path = Path.Combine(_env.WebRootPath, "images", "topics");
            await _fileUploadService.UploadAsync(path, fileName, model.ImageFile);

            var relativePath = "/images/topics/" + fileName;
            topic.Image = relativePath;
        }
    
        _topicRepository.Add(topic);
        await _topicRepository.SaveChangesAsync();
    
        foreach (var tagName in model.Tags)
        {
            var tag = await _tagRepository.GetByNameAsync(tagName);
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _tagRepository.Add(tag);
                await _tagRepository.SaveChangesAsync();
            }
        
            var topicTag = new TopicTag { TopicId = topic.Id, TagId = tag.Id };
            _topicTagRepository.Add(topicTag);
        }
        await _topicTagRepository.SaveChangesAsync();

        return RedirectToAction("Index", "Category");
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != topic.UserId)
        {
            return NotFound("User hasn't right to edit this post");
        }

        TopicEditViewModel model = new TopicEditViewModel
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            CategoryId = topic.CategoryId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TopicEditViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid)
        {
            var topic = await _topicRepository.GetByIdAsync(model.Id);

            if (userId != topic.UserId)
                return NotFound("User hasn't right to edit this post");

            if (topic == null)
            {
                return NotFound();
            }

            topic.Title = model.Title;
            topic.Content = model.Content;
            
            _topicRepository.Update(topic);

            if (await _topicRepository.SaveChangesAsync())
            {
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return BadRequest($"Failed!!!");
            }
        }

        return View(model);
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
    [Route("Topics/RateTopic/{topicId}")]
    public async Task<IActionResult> RateTopic(int topicId, RatingViewModel rating)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = new Rating { TopicId = topicId, Score = rating.Score, UserId = userId };
        _ratingRepository.Add(result);
        await _ratingRepository.SaveChangesAsync();
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
        return RedirectToAction("Index", "Topics");
    }
}