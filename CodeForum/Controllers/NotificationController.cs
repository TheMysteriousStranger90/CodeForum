using System.Security.Claims;
using CodeForum.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeForum.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationController(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);

        return View(notifications);
    }
    
    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notification = await _notificationRepository.GetByIdAsync(id);
        
        if (notification == null || notification.UserId != userId)
            return NotFound();

        notification.IsRead = true;
        _notificationRepository.Update(notification);
        await _notificationRepository.SaveChangesAsync();

        return Json(new { success = true });
    }
}