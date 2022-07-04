using RazorApp1.Models;

namespace RazorApp1.Domain.Services
{
    public interface ICatalogService
    {
        public IReadOnlyCollection<Product> Products { get; }
        public Task<bool> AddAsync(Product product, CancellationToken token = default);
        public Task<bool> RemoveAsync(int id, CancellationToken token = default);
    }
}
