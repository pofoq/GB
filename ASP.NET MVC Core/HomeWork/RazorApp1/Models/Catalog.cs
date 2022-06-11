
namespace RazorApp1.Models
{
    public class Catalog
    {
        public ConcurrentList<Good> Goods { get; set; } = new();
    }
}
