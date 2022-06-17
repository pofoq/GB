using Microsoft.AspNetCore.Mvc;
using RazorApp1.Domain;
using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorApp1.Controllers
{
    public class FileReaderController : Controller
    {
        private IFileReaderService _service;
        private ILogger<FileReaderController> _logger;

        public FileReaderController(IFileReaderService service, ILogger<FileReaderController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult FileText()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FileText([FromForm]string filePath)
        {
            _logger.LogDebug($"FilePath input: {filePath}");
            if (string.IsNullOrEmpty(filePath))
            {
                return View();
            }
            var model = _service.GetAllLinesAsync(filePath);
            return View(model: model);
        }
    }
}
