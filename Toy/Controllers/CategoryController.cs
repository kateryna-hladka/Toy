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
        public IActionResult Index(int id)
        {
            using (ToyContext toyContext = new())
            {
                var products = from product in toyContext.Products
                               join photo_product in toyContext.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                               from photo_product_result in ppj.DefaultIfEmpty()
                               let discount = toyContext.ProductDiscounts
                               .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                               let mark = toyContext.Reviews.Where(r=> r.ProductId == product.Id).Average(r=> r.Mark)
                               let comment = toyContext.Reviews.Where(r => r.ProductId == product.Id).Count()

                               where product.CategoryId == id && (photo_product_result.Photo.IsMain == true || photo_product_result == null)
                               select new
                               {
                                   product,
                                   UnitPriceName = product.PriceUnit.Name,
                                   photo_product_result.Photo.PhotoUrl,
                                   Discount = discount.Discount.Value as decimal?,
                                   UnitDiscountName = discount.Discount.Unit.Name,
                                   DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                                   DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?,
                                   MarkAvg = mark as double?,
                                   CommentCount = comment as int?
                               };
                IEnumerable<dynamic> productsDate = products.ToList();

                return View(productsDate);
            }

        }
    }
}
