using MediatR;
using RazorApp1.Domain.Events;
using RazorApp1.Domain.Services.MailService;

namespace RazorApp1.Domain.Handlers
{
    public class ProductDeletedEventHandler : INotificationHandler<ProductDeleted>
    {
        private readonly ILogger<ProductDeletedEventHandler> _logger;
        private readonly IMailService _mailService;
        private readonly CancellationToken _token;

        public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger, IMailService mailService, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _mailService = mailService;
            _token = accessor?.HttpContext?.RequestAborted ?? default;
        }

        public async Task Handle(ProductDeleted notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var token = CancellationTokenSource.CreateLinkedTokenSource(_token, cancellationToken).Token;

            string subject = "Product Deleted";
            string body = $"Id: {notification.Id}";

            await _mailService.SendEmailAsync(subject, body, token);
            _logger.LogInformation("{s}. {b}", subject, body);
        }
    }
}
