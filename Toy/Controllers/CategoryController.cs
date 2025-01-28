using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using Toy.Models;
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
            Utilit.DataBaseHelper dataBaseHelper = new();
            IEnumerable<dynamic> productsDate = dataBaseHelper.GetProduct(id, null);
            return View(productsDate);
        }
    }
}
