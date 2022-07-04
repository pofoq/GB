using RazorApp1.Models;
using System.Collections.Concurrent;

namespace RazorApp1
{
    public class Data
    {
        public Catalog Catalog = new();
        public ConcurrentDictionary<string, int> Metrics = new();
    }
}
