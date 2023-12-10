using System.Security.Claims;
using CodeForum.Helpers;
using CodeForum.Interfaces;
using CodeForum.Models;
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
    private readonly IFileUploadService _uploadService;
    private readonly IWebHostEnvironment _env;

    public PostsController(ITopicRepository topicRepository, IPostRepository postRepository,
        ILikeDislikeRepository likeDislikeRepository,
        IReportRepository reportRepository, IFileUploadService uploadService, IWebHostEnvironment env)
    {
        _topicRepository = topicRepository;
        _postRepository = postRepository;
        _likeDislikeRepository = likeDislikeRepository;
        _reportRepository = reportRepository;
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
        }
        
        ViewData["Reports"] = reports;

        return View(pagedPosts);
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
        return RedirectToAction("Index", "Topics");
    }
}