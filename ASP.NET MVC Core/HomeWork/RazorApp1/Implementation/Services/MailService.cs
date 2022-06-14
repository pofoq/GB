using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorApp1.Domain;
using RazorApp1.Domain.Services.MailService;
using System.ComponentModel.DataAnnotations;
namespace RazorApp1.Implementation.Services
{
    public class MailService : IMailService, IDisposable
    {
        public string RecipientEmail
        {
            get
            {
                if (_context.Request.Cookies.ContainsKey(DataKey.Email))
                {
                    return _context.Request.Cookies[DataKey.Email] ?? "";
                }
                return "";
            }
            set
            {
                if (!_context.Request.Cookies.ContainsKey(DataKey.Email))
                {
                    _context.Response.Cookies.Delete(DataKey.Email);
                }

                _context.Response.Cookies.Append(DataKey.Email, value);
            }
        }

        private SmtpClient _smtpClient;
        private string _name;
        private string _from;
        private string _password;
        private string _server;
        private int _port;
        private bool _useSsl;
        private HttpContext _context;

        public MailService(IOptions<MailOption> option, IHttpContextAccessor context)
        {
            ArgumentNullException.ThrowIfNull(context?.HttpContext);

            _name = option.Value.Name;
            _from = option.Value.From;
            _password = option.Value.Password;
            _server = option.Value.Server;
            _port = option.Value.Port;
            _useSsl = option.Value.UseSsl;
            _context = context.HttpContext;
            _smtpClient = new SmtpClient();
        }

        public async Task SendEmailAsync(string address, string subject, string body, CancellationToken token = default)
        {
            if (address == null || !new EmailAddressAttribute().IsValid(address))
            {
                return;
            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_name, _from));
            message.To.Add(new MailboxAddress("", address));
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
        }

        public async Task SendEmailAsync(string subject, string body, CancellationToken token = default)
        {
            await SendEmailAsync(RecipientEmail, subject, body, token);
        }

        public async void Dispose()
        {
            if (_smtpClient.IsConnected)
            {
                await _smtpClient.DisconnectAsync(true, _context.RequestAborted);
            }

            _smtpClient.Dispose();
        }
    }
}
