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
            _catalog.Goods.Add(good);
            return View(model: $"Good added. Total goods: {_catalog.Goods.Count()}");
        }
    }
}
