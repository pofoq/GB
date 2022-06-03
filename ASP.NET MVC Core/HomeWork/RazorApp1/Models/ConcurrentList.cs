
using System.Collections;

namespace RazorApp1.Models
{
    public class ConcurrentList<T> : IEnumerable<T>
    {
        private readonly List<T> _list = new();
        private readonly object _lock = new object();

        public void Add(T obj)
        {
            lock (_lock)
            {
                _list.Add(obj);
            }
        }

        public void Remove(T obj)
        {
            lock (_lock)
            {
                _list.Remove(obj);
            }
        }

        public List<T> Sort()
        {
            lock (_lock)
            {
                _list.Sort();
            }
            return _list;
        }

        public int Count()
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return _list.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_lock)
            {
                return _list.GetEnumerator();
            }
        }
    }
}
