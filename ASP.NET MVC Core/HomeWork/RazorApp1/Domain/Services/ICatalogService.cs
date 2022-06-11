using RazorApp1.Models;

namespace RazorApp1.Domain.Services
{
    public interface ICatalogService
    {
        public IReadOnlyCollection<Good> Goods { get; }
        public Task<bool> AddAsync(Good good);
        public Task<bool> RemoveAsync(int id);
    }
}
