using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorApp1.Domain;
using RazorApp1.Domain.Services.MailService;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace RazorApp1.Implementation.Services
{
    public class MailService : IMailService
    {
        public string RecipientEmail
        {
            get
            {
                if (_context != null && _context.Request.Cookies.ContainsKey(DataKey.Email))
                {
                    return _context.Request.Cookies[DataKey.Email] ?? "";
                }
                return _mailOption.To;
            }
            set
            {
                if (_context != null)
                {
                    if (_context.Request.Cookies.ContainsKey(DataKey.Email))
                    {
                        _context.Response.Cookies.Delete(DataKey.Email);
                    }

                    _context.Response.Cookies.Append(DataKey.Email, value);
                }
            }
        }

        private readonly MailOption _mailOption;
        private readonly SmtpClient _smtpClient;
        private readonly string _name;
        private readonly string _from;
        private readonly string _password;
        private readonly string _server;
        private readonly int _port;
        private readonly bool _useSsl;
        private readonly ILogger<MailService> _logger;
        private readonly HttpContext? _context;

        public MailService(IOptions<MailOption> option, ILogger<MailService> logger, IHttpContextAccessor accessor)
        {
            _context = accessor?.HttpContext;
            _mailOption = option.Value;
            _name = option.Value.Name;
            _from = option.Value.From;
            _password = option.Value.Password;
            _server = option.Value.Server;
            _port = option.Value.Port;
            _useSsl = option.Value.UseSsl;
            _smtpClient = new SmtpClient();
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken token = default)
        {
            var sw = Stopwatch.StartNew();
            token.ThrowIfCancellationRequested();

            if (to == null || !new EmailAddressAttribute().IsValid(to))
            {
                return;
            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_name, _from));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_server, _port, _useSsl, token);
            }
            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(_from, _password, token);
            }
            await _smtpClient.SendAsync(message, token);
            _logger.LogInformation("Mail sent to {to} from {from}. Duration: {sw} miliseconds", to, message.From, sw.Elapsed.Milliseconds);
        }

        public async Task SendEmailAsync(string subject, string body, CancellationToken token = default)
        {
            await SendEmailAsync(RecipientEmail, subject, body, token);
        }

        public void Dispose()
        {
            try
            {
                if (_smtpClient.IsConnected)
                {
                    _smtpClient.Disconnect(true);
                }

                _smtpClient.Dispose();
                GC.SuppressFinalize(this);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{method} exception: {ex}", nameof(Dispose), ex.Message);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_smtpClient.IsConnected)
                {
                    await _smtpClient.DisconnectAsync(true);
                }

                _smtpClient.Dispose();
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{method} exception: {ex}", nameof(DisposeAsync), ex.Message);
            }
        }
    }
}
