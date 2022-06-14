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
        private ILogger<CatalogController> _logger;

        public CatalogController(ICatalogService catalogService, IMailService mailService, ILogger<CatalogController> logger)
        {
            _catalogService = catalogService;
            _mailService = mailService;
            _logger = logger;
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

            _logger.LogDebug("Debug: new email set to:" + email);

            return View(viewName: "Goods");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Good good)
        {
            var result = await _catalogService.AddAsync(good);
            var msg = result ? $"Good is added. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            _logger.LogInformation(msg);
            return View(viewName: "Goods", model: msg);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm] int id)
        {
            var result = await _catalogService.RemoveAsync(id);
            var msg = result ? "Good is delted. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            _logger.LogInformation(msg);
            return View(viewName: "Goods", model: msg);
        }
    }
}
