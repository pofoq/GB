using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Models;

namespace RazorApp1.Implementation.Services
{
    public class CatalogService : ICatalogService
    {
        private Catalog _catalog;
        private IMailService _mailService;

        public CatalogService(IMailService mailService, Data data)
        {
            _mailService = mailService;
            _catalog = data.Catalog;
        }

        public IReadOnlyCollection<Good> Goods => _catalog.Goods;

        public async Task<bool> AddAsync(Good good)
        {
            var result = _catalog.Add(good);

            var msg = result ? "Good is added." : "Something wrong.";

            await _mailService.SendEmailAsync("Add Good.", $"<h3> {msg} </h3>");
            return result;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var result = _catalog.Remove(id);
            var msg = result ? "Delete Good." : "Something wrong.";

            await _mailService.SendEmailAsync(msg, $"<h2> {msg} </h2>");
            return result;
        }
    }
}
