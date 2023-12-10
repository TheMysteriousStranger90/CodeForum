using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CodeForum.Interfaces;

namespace CodeForum.Controllers;

[Authorize(Roles = "Administrator")]
public class ReportsController : Controller
{
    private readonly IReportRepository _reportRepository;
    private readonly IPostRepository _postRepository;

    public ReportsController(IReportRepository reportRepository, IPostRepository postRepository)
    {
        _reportRepository = reportRepository;
        _postRepository = postRepository;
    }

    public async Task<IActionResult> Index()
    {
        var reports = await _reportRepository.GetAllReportsAsync();
        return View(reports);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        var reports = await _reportRepository.GetReportsByPostIdAsync(id);

        foreach (var report in reports)
        {
            _reportRepository.Delete(report);
        }

        await _reportRepository.SaveChangesAsync();

        _postRepository.Delete(post);
        await _postRepository.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}