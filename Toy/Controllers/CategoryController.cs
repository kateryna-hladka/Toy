using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using Toy.Models;
using Toy.Utilit.DataBase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Toy.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ToyContext _context;

        public CategoryController(ToyContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            DataBaseHelper dataBaseHelper = new();
            IEnumerable<dynamic> productsDate = dataBaseHelper.GetProduct(id, null);

            Dictionary<string, object> result = new ();
            result.Add("product", productsDate);
            result.Add("basketsId", dataBaseHelper.GetProductsId(Request));

            return View(result);
        }
    }
}
