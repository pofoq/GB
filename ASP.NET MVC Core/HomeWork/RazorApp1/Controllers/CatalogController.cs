using MediatR;
using Microsoft.AspNetCore.Mvc;
using RazorApp1.Domain;
using RazorApp1.Domain.Services;
using RazorApp1.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorApp1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<CatalogController> _logger;
        private readonly CancellationToken _token;
        private readonly ContextData _contextData;
        private readonly IMediator _mediator;

        public CatalogController(
            ICatalogService catalogService, 
            ILogger<CatalogController> logger,
            IHttpContextAccessor accessor,
            ContextData contextData,
            IMediator mediator)
        {
            _catalogService = catalogService;
            _logger = logger;
            _token = accessor?.HttpContext?.RequestAborted ?? default;
            _contextData = contextData;
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Catalog()
        {
            return View(model: _catalogService.Products);
        }

        [HttpGet]
        public IActionResult Products()
        {
            ViewData[DataKey.Email] = _contextData.RecipientEmail;
            return View();
        }

        [HttpPost]
        public IActionResult Email([FromForm][EmailAddress] string email)
        {
            _contextData.RecipientEmail = email;
            ViewData[DataKey.Email] = $"New Email: {email}";

            _logger.LogDebug("New email set to: {email}", email);

            return View(viewName: "Products");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Product product)
        {
            var isAdded = await _catalogService.AddAsync(product, _token);
            var msg = isAdded ? $"Product is added. Total products: {_catalogService.Products.Count}." : "Something wrong.";
            _logger.LogInformation("{add}:: {msg}", nameof(Add), msg);
            return View(viewName: "Products", model: msg);
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm] int id)
        {
            var result = await _catalogService.RemoveAsync(id);
            var msg = result ? $"Product is delted. Total products: {_catalogService.Products.Count}." : "Something wrong.";
            _logger.LogInformation("{method}:: {message}", nameof(Remove), msg);
            return View(viewName: "Products", model: msg);
        }
    }
}
