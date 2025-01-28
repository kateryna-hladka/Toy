using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Toy.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Toy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ToyContext _context;

        public HomeController(ILogger<HomeController> logger, ToyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
                List<Category> categories = [.. _context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => c)];

                Dictionary<string, object> result = new();
                result.Add("Category", categories);

                return View(result);
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
