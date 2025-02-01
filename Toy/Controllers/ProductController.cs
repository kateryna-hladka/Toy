using Microsoft.AspNetCore.Mvc;
using Toy.Models;
using Toy.Utilit.DataBase;

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
            DataBaseHelper dataBaseHelper = new ();
            var product = dataBaseHelper.GetProduct(null, id);

            
            Dictionary<string, object> result = new();
            result.Add("product", product);
            result.Add("basketsId", dataBaseHelper.IsProductInBasket(Request, id));
            return View(result);
            
        }
        public IActionResult Buy()
        {
            return View();
        }
    }
}
