using Microsoft.AspNetCore.Mvc;
using RazorApp1.Domain;
using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorApp1.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogService;
        private IMailService _mailService;

        public CatalogController(ICatalogService catalogService, IMailService mailService)
        {
            _catalogService = catalogService;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Catalog()
        {
            return View(model: _catalogService.Goods);
        }

        [HttpGet]
        public IActionResult Goods()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Email([FromForm][EmailAddress] string email)
        {
            _mailService.RecipientEmail = email;
            ViewData[DataKey.Email] = $"New Email: {email}";
            return View(viewName: "Goods");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Good good)
        {
            var result = await _catalogService.AddAsync(good);
            var msg = result ? $"Good is added. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            return View(viewName: "Goods", model: msg);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm] int id)
        {
            var result = await _catalogService.RemoveAsync(id);
            var msg = result ? "Good is delted. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            return View(viewName: "Goods", model: msg);
        }

        private void SetEmail(string email)
        {
            if (!Request.Cookies.ContainsKey(DataKey.Email))
            {
                Response.Cookies.Delete(DataKey.Email);
            }

            Response.Cookies.Append(DataKey.Email, email);
        }

        private string GetEmail()
        {
            if (Request.Cookies.ContainsKey(DataKey.Email))
            {
                return Request.Cookies[DataKey.Email] ?? "";
            }
            return "";
        }
    }
}
