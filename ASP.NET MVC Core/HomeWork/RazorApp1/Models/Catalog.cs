using System.Collections.Concurrent;

namespace RazorApp1.Models
{
    public class Catalog
    {
        private readonly ConcurrentDictionary<int, Good> _goods = new();

        public bool Add(Good good) => _goods.TryAdd(good.Id, good);
        public bool Remove(int id) => _goods.TryRemove(id, out _);
        public bool Remove(Good good) => Remove(good.Id);
        public int Count => _goods.Count;
        public IReadOnlyCollection<Good> Goods => _goods.Values.ToArray();
    }
}
