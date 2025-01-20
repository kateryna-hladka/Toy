using Microsoft.AspNetCore.Mvc;

namespace Toy.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
