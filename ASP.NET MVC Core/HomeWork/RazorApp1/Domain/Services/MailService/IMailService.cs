
namespace RazorApp1.Domain.Services.MailService
{
    public interface IMailService : IAsyncDisposable, IDisposable
    {
        public string RecipientEmail { get; }
        public Task SendEmailAsync(string address, string subject, string body, CancellationToken token = default);
        public Task SendEmailAsync(string subject, string body, CancellationToken token = default);
    }
}
