using Microsoft.AspNetCore.Mvc;
using Toy.Models;

namespace Toy.Controllers
{
    public class ProductController : Controller
    {
        private readonly ToyContext _context;
        public ProductController(ToyContext context)
        {
            _context= context;
        }

        public IActionResult Index(int id)
        {
            Utilit.DataBaseHelper dataBaseHelper = new ();
            var product = dataBaseHelper.GetProduct(null, id);

            return View(product);
            
        }
        public IActionResult Buy()
        {
            return View();
        }
    }
}
