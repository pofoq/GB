using RazorApp1.Domain.Services;

namespace RazorApp1.Implementation.Services
{
    public class FileReaderService : IFileReaderService
    {
        public List<string> FilePath { get; set; }
        private ILogger<FileReaderService> _logger;

        public FileReaderService(ILogger<FileReaderService> logger)
        {
            _logger = logger;
            FilePath = new();
        }

        public async IAsyncEnumerable<string> GetAllLinesAsync(params string[] filePath)
        {
            if (filePath?.Length == 1)
            {
                filePath = filePath[0].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (var path in filePath ?? Array.Empty<string>())
            {
                if (File.Exists(path))
                {
                    _logger.LogInformation($"Read: {path}");
                    using var reader = File.OpenText(path);
                    while (!reader.EndOfStream)
                    {
                        yield return await reader.ReadLineAsync() ?? "";
                    }
                }
                else
                {
                    _logger.LogWarning($"File Not Found {path}");
                    yield return $"File Not Found {path}";
                }
            }
        }
    }
}
