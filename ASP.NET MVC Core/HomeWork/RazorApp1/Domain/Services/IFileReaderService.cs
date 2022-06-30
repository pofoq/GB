namespace RazorApp1.Domain.Services
{
    public interface IFileReaderService
    {
        IAsyncEnumerable<string> GetAllLinesAsync(params string[] filePath);
    }
}
