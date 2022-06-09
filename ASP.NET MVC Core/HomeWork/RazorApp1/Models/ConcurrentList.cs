using System.Collections;
using System.Collections.Concurrent;

namespace RazorApp1.Models
{
    public class ConcurrentList<T> : IEnumerable<T>
    {
        private readonly ConcurrentDictionary<int,T> _list = new();

        public bool Add(int id, T obj)
        {
            return _list.TryAdd(id, obj);
        }

        public bool Remove(int id)
        {
            return _list.TryRemove(_list.FirstOrDefault(el => el.Key == id));
        }

        public int Count()
        {
            return _list.Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }
    }
}
