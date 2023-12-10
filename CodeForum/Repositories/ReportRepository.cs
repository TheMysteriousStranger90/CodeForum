using CodeForum.Context;
using CodeForum.Interfaces;
using CodeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeForum.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Report>> GetReportsByPostIdAsync(int postId)
    {
        return await _context.Reports
            .Where(r => r.PostId == postId)
            .ToListAsync();
    }

    public async Task<Report> GetReportByPostIdAndUserId(int postId, string userId)
    {
        return await _context.Reports
            .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
    }

    public async Task<IEnumerable<Report>> GetAllReportsAsync()
    {
        return await _context.Reports
            .Include(r => r.Post)
            .ThenInclude(p => p.User)
            .Include(r => r.User)
            .ToListAsync();
    }
}