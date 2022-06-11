using Microsoft.AspNetCore.Mvc;
using RazorApp1.Models;

namespace RazorApp1.Controllers
{
    public class CatalogController : Controller
    {
        private static Catalog _catalog = Data.Catalog;

        [HttpGet]
        public IActionResult Catalog()
        {
            return View(_catalog);
        }

        [HttpGet]
        public IActionResult Goods()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Goods([FromForm] Good good)
        {
            _catalog.Add(good);
            return View(model: $"Good added. Total goods: {_catalog.Count}");
        }

        [HttpPost]
        public IActionResult Remove([FromForm] int id)
        {
            var result = _catalog.Remove(id) ? "Good is delted." : "Something wrong.";
            return View("Goods", model: $"Result: {result}");
        }
    }
}
