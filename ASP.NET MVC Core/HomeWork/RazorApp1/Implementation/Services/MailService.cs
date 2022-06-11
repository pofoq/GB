using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorApp1.Domain;
using RazorApp1.Domain.Services.MailService;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace RazorApp1.Implementation.Services
{
    //smtp.beget.com (порт 25) Логин: asp2022gb@rodion-m.ru Пароль: 3drtLSa1
    public class MailService : IMailService
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

        private string _name;
        private string _from;
        private string _password;
        private string _server;
        private int _port;
        private bool _useSsl;
        private HttpContext _context;

        public MailService(IOptions<MailOption> option, IHttpContextAccessor context)//, HttpRequest request, HttpResponse response)
        {
            _name = option.Value.Name;
            _from = option.Value.From;
            _password = option.Value.Password;
            _server = option.Value.Server;
            _port = option.Value.Port;
            _useSsl = option.Value.UseSsl;
            _context = context?.HttpContext;
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

            using var client = new SmtpClient();

            await client.ConnectAsync(_server, _port, _useSsl, token);
            await client.AuthenticateAsync(_from, _password, token);
            await client.SendAsync(message, token);
            await client.DisconnectAsync(true, token);
        }

        public async Task SendEmailAsync(string subject, string body, CancellationToken token = default)
        {
            await SendEmailAsync(RecipientEmail, subject, body, token);
        }
    }
}
