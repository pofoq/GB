using System.Collections.Concurrent;

namespace RazorApp1.Models
{
    public class Catalog
    {
        private readonly ConcurrentDictionary<int, Product> _products = new();

        public bool Add(Product product) => _products.TryAdd(product.Id, product);
        public bool Remove(int id) => _products.TryRemove(id, out _);
        public bool Remove(Product product) => Remove(product.Id);
        public int Count => _products.Count;
        public IReadOnlyCollection<Product> Products => _products.Values.ToArray();
    }
}
