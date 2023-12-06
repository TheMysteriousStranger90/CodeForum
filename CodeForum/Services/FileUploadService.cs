using CodeForum.Interfaces;

namespace CodeForum.Services;

public class FileUploadService : IFileUploadService
{
    public async Task UploadAsync(string path, string fileName, IFormFile file)
    {
        using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
}