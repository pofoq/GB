using RazorApp1.Domain.Services;
using RazorApp1.Models;
using MediatR;
using RazorApp1.Domain.Events;

namespace RazorApp1.Implementation.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly Catalog _catalog;
        private readonly IMediator _mediator;

        public CatalogService(IMediator mediator, Data data)
        {
            _mediator = mediator;
            _catalog = data.Catalog;
        }

        public IReadOnlyCollection<Product> Products => _catalog.Products;

        public async Task<bool> AddAsync(Product product, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            var isAdded = _catalog.Add(product);

            if (isAdded)
            {
                var notification = new ProductAdded(product);
                await _mediator.Publish(notification, token);
            }

            return isAdded;
        }

        public async Task<bool> RemoveAsync(int id, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            var isDeleted = _catalog.Remove(id);

            if (isDeleted)
            {
                var notification = new ProductDeleted(id);
                await _mediator.Publish(notification, token);
            }

            return isDeleted;
        }
    }
}
