using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Toy.Models;
using static Toy.Controllers.BasketController;

namespace Toy.Controllers
{
    public class BasketController : Controller
    {
        private readonly ToyContext _context;

        public BasketController(ToyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (Request.Cookies["login"] != null)
            {

                IEnumerable<dynamic> basket = from b in _context.Baskets
                                              where b.User.Email == Request.Cookies["login"] || b.User.Phone == Request.Cookies["login"]
                                              join products in _context.Products on b.ProductId equals products.Id into pj
                                              from product in pj.DefaultIfEmpty()
                                              join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                                              from photo_product_result in ppj.DefaultIfEmpty()
                                              let discount = _context.ProductDiscounts
                                              .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                                              select new
                                              {
                                                  product,
                                                  photo_product_result.Photo.PhotoUrl,
                                                  UnitPriceName = product.PriceUnit.Name,
                                                  Discount = discount.Discount.Value as decimal?,
                                                  UnitDiscountName = discount.Discount.Unit.Name,
                                                  DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                                                  DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?
                                              };

                return View(basket);

            }
            if (HttpContext.Session.GetString($"{HttpContext.Session.GetString("newUser")}_basket") != null)
            {
                IQueryable<int> productsId = _context.Products.Select(p => p.Id);
                List<int> Ids = new();
                foreach (var pId in productsId)
                {
                    int? value = HttpContext.Session.GetInt32($"product_{HttpContext.Session.GetString("newUser")}_{pId}");
                    if (value != null)
                        Ids.Add(Convert.ToInt32(value));
                }



                IEnumerable<dynamic> basket = from product in _context.Products
                                              where Ids.Contains(product.Id)
                                              join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                                              from photo_product_result in ppj.DefaultIfEmpty()
                                              let discount = _context.ProductDiscounts
                                              .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                                              select new
                                              {
                                                  product,
                                                  photo_product_result.Photo.PhotoUrl,
                                                  UnitPriceName = product.PriceUnit.Name,
                                                  Discount = discount.Discount.Value as decimal?,
                                                  UnitDiscountName = discount.Discount.Unit.Name,
                                                  DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                                                  DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?
                                              };

                return View(basket);

            }
            return View();
        }
        [HttpPost]
        public IActionResult Add([FromBody] ProductRequest productRequest)
        {
            if (productRequest?.ProductId is not int || productRequest.ProductId <= 0)
                return Json(new { success = false, message = "Invalid product ID" });



            bool isAmount = productRequest.Amount != null;
            bool amountLessProductAmount = true;

            if (isAmount)
            {
                short amount = _context.Products
                    .Where(p => p.Id == productRequest.ProductId)
                    .Select(p => p.Amount)
                    .FirstOrDefault();
                if (amount < productRequest.Amount)
                    amountLessProductAmount = false;
            }

            if ((isAmount && productRequest?.Amount is not short) ||
                 (isAmount && productRequest.Amount <= 0) ||
                 (isAmount && !amountLessProductAmount))
                return Json(new { success = false, message = "Invalid product amount" });

            if (Request.Cookies["login"] != null)
            {

                _context.Baskets.Add(new Basket()
                {
                    ProductId = productRequest.ProductId,
                    UserId = (_context.User.Where(u => u.Phone == Request.Cookies["login"] || u.Email == Request.Cookies["login"]).Select(u => u.Id).FirstOrDefault()),
                    Amount = Convert.ToInt16(productRequest.Amount)
                });
                _context.SaveChanges();

            }
            else
            {
                string sessionKey = $"product_{HttpContext.Session.GetString("newUser")}_{productRequest.ProductId}";
                HttpContext.Session.SetInt32(sessionKey, productRequest.ProductId);

                string sessionAmountKey = $"product_{HttpContext.Session.GetString("newUser")}_{productRequest.ProductId}_count";
                HttpContext.Session.SetInt32(sessionAmountKey, productRequest.Amount ?? 1);
                HttpContext.Session.SetString($"{HttpContext.Session.GetString("newUser")}_basket", "has");
            }
            return Json(new { success = true });
        }
        public class ProductRequest
        {
            public int ProductId { get; set; }
            public short? Amount { get; set; }
        }
    }
}
