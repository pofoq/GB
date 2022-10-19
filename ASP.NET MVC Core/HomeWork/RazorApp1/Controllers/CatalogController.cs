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
        private readonly ICatalogService _catalogService;
        private readonly IMailService _mailService;
        private readonly ILogger<CatalogController> _logger;
        private readonly CancellationToken _token;

        public CatalogController(
            ICatalogService catalogService, 
            IMailService mailService, 
            ILogger<CatalogController> logger, 
            IHttpContextAccessor accessor)
        {
            _catalogService = catalogService;
            _mailService = mailService;
            _logger = logger;
            _token = accessor?.HttpContext?.RequestAborted ?? default;
        }

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
            ViewData[DataKey.Email] = _mailService.RecipientEmail;
            return View();
        }

        [HttpPost]
        public IActionResult Email([FromForm][EmailAddress] string email)
        {
            _mailService.RecipientEmail = email;
            ViewData[DataKey.Email] = $"New Email: {email}";

            _logger.LogDebug("New email set to: {email}", email);

            return View(viewName: "Goods");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Good good)
        {
            var result = await _catalogService.AddAsync(good, _token);
            var msg = result ? $"Good is added. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            _logger.LogInformation("{add}:: {msg}", nameof(Add), msg);
            return View(viewName: "Goods", model: msg);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm] int id)
        {
            var result = await _catalogService.RemoveAsync(id);
            var msg = result ? "Good is delted. Total goods: {_catalogService.Goods.Count}." : "Something wrong.";
            _logger.LogInformation("{method}:: {message}", nameof(Remove), msg);
            return View(viewName: "Goods", model: msg);
        }
    }
}
