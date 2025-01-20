using Microsoft.AspNetCore.Mvc;

namespace Toy.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
