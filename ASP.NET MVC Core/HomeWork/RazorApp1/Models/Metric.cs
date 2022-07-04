
namespace RazorApp1.Models
{
    public class Metric
    {
        public string Path { get; set; } = "";
        public int Count { get; set; } = 0;

        public Metric(string path, int count)
        {
            Path = path;
            Count = count;
        }
    }
}
