using Microsoft.AspNetCore.Mvc;
using RazorApp1.Models;

namespace RazorApp1.Controllers
{
    public class MetricsController : Controller
    {
        private readonly Data _data;

        public MetricsController(Data data)
        {
            _data = data;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IReadOnlyCollection<Metric> counter = _data.Metrics.Select(el => new Metric(el.Key, el.Value)).ToArray();
            return View(model: counter);
        }
    }
}
