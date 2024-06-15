using CsvReaderApp.Models;
using CsvReaderApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsvReaderApp.Controllers
{
    [Route("[controller]")]
    public class CsvImporterController : Controller
    {
        private readonly CsvService _csvService;

        public CsvImporterController(CsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(new List<User>());
        }

        [HttpPost("")]
        [HttpPost("Index")]
        public IActionResult Index(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    try
                    {
                        var users = _csvService.ReadCsvFile(stream).ToList();
                        return View(users); // Return users list to the view
                    }
                    catch (ApplicationException ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
            }

            return View(new List<User>());
        }

    }
}
