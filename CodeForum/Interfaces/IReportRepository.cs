using CodeForum.Models;

namespace CodeForum.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<IEnumerable<Report>> GetReportsByPostIdAsync(int postId);
    Task<Report> GetReportByPostIdAndUserId(int postId, string userId);
    Task<IEnumerable<Report>> GetAllReportsAsync();
}