using System.Security.Claims;
using CodeForum.Helpers;
using CodeForum.Interfaces;
using CodeForum.Models;
using CodeForum.Models.ViewModels;
using CodeForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CodeForum.Controllers;

[Authorize]
public class PostsController : Controller
{
    private readonly ITopicRepository _topicRepository;
    private readonly IPostRepository _postRepository;
    private readonly ILikeDislikeRepository _likeDislikeRepository;
    private readonly IReportRepository _reportRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IFileUploadService _uploadService;
    private readonly IWebHostEnvironment _env;

    public PostsController(ITopicRepository topicRepository, IPostRepository postRepository,
        ILikeDislikeRepository likeDislikeRepository,
        IReportRepository reportRepository, INotificationRepository notificationRepository,
        IFileUploadService uploadService, IWebHostEnvironment env)
    {
        _topicRepository = topicRepository;
        _postRepository = postRepository;
        _likeDislikeRepository = likeDislikeRepository;
        _reportRepository = reportRepository;
        _notificationRepository = notificationRepository;
        _uploadService = uploadService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id, [FromQuery] PaginationParams paginationParams)
    {
        var topic = await _topicRepository.GetByIdAsync(id);

        if (topic == null)
        {
            return NotFound();
        }

        var postsQuery = _postRepository.GetPostsForTopic(topic.Id);

        var pagedPosts =
            await PagedList<Post>.CreateAsync(postsQuery, paginationParams.PageNumber, paginationParams.PageSize);

        var paginationHeader = new PaginationHeader(pagedPosts.CurrentPage, pagedPosts.PageSize, pagedPosts.TotalCount,
            pagedPosts.TotalPages);

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

        ViewBag.TopicId = id;

        var reports = new Dictionary<int, bool>();
        foreach (var post in pagedPosts)
        {
            var report =
                await _reportRepository.GetReportByPostIdAndUserId(post.Id,
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
            reports[post.Id] = report != null;

            var likesDislikes = await _likeDislikeRepository.GetLikesDislikesByPostIdAsync(post.Id);
            ViewData["LikesDislikes" + post.Id] = likesDislikes;
        }

        ViewData["Reports"] = reports;

        return View(pagedPosts);
    }

    [HttpGet]
    public IActionResult Create(int? topicId)
    {
        if (topicId == null) return RedirectToAction("Index");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = new Post
        {
            Content = model.Content,
            TopicId = model.TopicId,
            UserId = userId,
            CreatedAt = DateTime.Now,
        };

        if (model.ImageFile != null)
        {
            var filePath = await _uploadService.UploadFileWithoutPathAsync(model.ImageFile);
            post.Image = filePath;
        }
        else
        {
            post.Image = "";
        }

        _postRepository.Add(post);
        await _postRepository.SaveChangesAsync();
        await _notificationRepository.CreatePostNotificationAsync(post);

        return RedirectToAction("Index", "Posts", new { id = model.TopicId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != post.UserId)
        {
            return NotFound("User hasn't right to edit this post");
        }

        PostEditViewModel model = new PostEditViewModel
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = DateTime.Now,
            TopicId = post.TopicId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PostEditViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid)
        {
            var post = await _postRepository.GetByIdAsync(model.Id);

            if (userId != post.UserId)
                return NotFound("User hasn't right to edit this post");

            if (post == null)
            {
                return NotFound();
            }

            post.Content = model.Content;

            _postRepository.Update(post);

            if (await _postRepository.SaveChangesAsync())
            {
                return RedirectToAction("Index", "Posts", new { id = post.TopicId });
            }
            else
            {
                return BadRequest($"Failed!!!");
            }
        }

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var post = await _postRepository.GetByIdAsync(id);

        if (userId != post.UserId)
            return NotFound("User hasn't right to delete this post");

        if (post != null)
        {
            _postRepository.Delete(post);
            await _topicRepository.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Posts", new { id = post.TopicId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Posts/ReportPost/{postId}")]
    public async Task<IActionResult> ReportPost(int postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post == null || userId == null)
        {
            return NotFound();
        }

        var existingFavorite = await _reportRepository.GetReportByPostIdAndUserId(post.Id, userId);
        if (existingFavorite != null)
        {
            _reportRepository.Delete(existingFavorite);
        }
        else
        {
            var report = new Report()
            {
                TopicId = post.TopicId, PostId = post.Id, UserId = userId, Content = post.Content,
                ReportedAt = DateTime.Now
            };
            _reportRepository.Add(report);
        }

        await _reportRepository.SaveChangesAsync();
        return RedirectToAction("Index", "Posts", new { id = post.TopicId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Posts/LikeDislikePost/{postId}")]
    public async Task<IActionResult> LikeDislikePost(int postId, bool isLike)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var existingLikeDislike = _likeDislikeRepository.GetLikesDislikesByUserIdAsync(userId)
            .Result
            .FirstOrDefault(ld => ld.PostId == postId);
        
        var post = await _postRepository.GetByIdAsync(postId);

        if (existingLikeDislike != null)
        {
            if (existingLikeDislike.IsLike == isLike)
            {
                _likeDislikeRepository.Delete(existingLikeDislike);
            }
            else
            {
                existingLikeDislike.IsLike = isLike;
                _likeDislikeRepository.Update(existingLikeDislike);
            }
        }
        else
        {
            var likeDislike = new LikeDislike { PostId = postId, UserId = userId, IsLike = isLike };
            _likeDislikeRepository.Add(likeDislike);
        }

        await _likeDislikeRepository.SaveChangesAsync();
        return RedirectToAction("Index", "Posts", new { id = post.TopicId });
    }
}