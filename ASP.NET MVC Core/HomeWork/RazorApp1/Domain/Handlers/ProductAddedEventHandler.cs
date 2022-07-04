using MediatR;
using RazorApp1.Domain.Events;
using RazorApp1.Domain.Services.MailService;

namespace RazorApp1.Domain.Handlers
{
    public class ProductAddedEventHandler : INotificationHandler<ProductAdded>
    {
        private readonly ILogger<ProductAddedEventHandler> _logger;
        private readonly IMailService _mailService;
        private readonly CancellationToken _token;

        public ProductAddedEventHandler(ILogger<ProductAddedEventHandler> logger, IMailService mailService, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _mailService = mailService;
            _token = accessor?.HttpContext?.RequestAborted ?? default;
        }

        public async Task Handle(ProductAdded notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var token = CancellationTokenSource.CreateLinkedTokenSource(_token, cancellationToken).Token;

            string subject = "Product Added";
            string body = $"Id: {notification.Product.Id}<br />Name: {notification.Product.Name}";

            await _mailService.SendEmailAsync(subject, body, token);
            _logger.LogInformation("{s}. {b}", subject, body);
        }
    }
}
