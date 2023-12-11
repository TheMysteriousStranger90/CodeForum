using CodeForum.Interfaces;

namespace CodeForum.Services;

public class FileUploadService : IFileUploadService
{
    public async Task UploadAsync(string path, string fileName, IFormFile file)
    {
        using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }
    
    public async Task<string> UploadFileWithoutPathAsync(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return "/images/" + fileName;
    }
}