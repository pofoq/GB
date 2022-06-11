using System.ComponentModel.DataAnnotations;

namespace RazorApp1.Domain.Services.MailService
{
    public class MailOption
    {
        public string Name { get; set; }
        [EmailAddress]
        public string From { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; } = 25;
        public bool UseSsl { get; set; } = false;

    }
}
