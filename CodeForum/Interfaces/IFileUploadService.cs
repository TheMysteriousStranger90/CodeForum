namespace CodeForum.Interfaces;

public interface IFileUploadService
{
    Task UploadAsync(string path, string fileName, IFormFile file);
    Task<string> UploadFileWithoutPathAsync(IFormFile file);
}