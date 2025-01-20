using Microsoft.AspNetCore.Mvc;
using Toy.Models;

namespace Toy.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(int id)
        {
            using (ToyContext toyContext = new())
            {
                Utilit.DataBaseHelper dataBaseHelper = new ();
                var product = dataBaseHelper.GetProduct(null, id);

                return View(product);
            }
        }
    }
}
