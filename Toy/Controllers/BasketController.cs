using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
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
                                                  MaxAmount = product.Amount,
                                                  b.Amount,
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
                string userId = HttpContext.Session.GetString($"newUser");
                Console.WriteLine(userId);
                List<string> sessionKeys = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
                Dictionary<int, short> idAmount = new ();
                int splId;
                foreach (string k in sessionKeys)
                    Console.WriteLine(k);
                //&& 

                /*
                 product_3aa11d82-7c28-7dea-b506-94aba5b9bc5a_16
                 product_3aa11d82-7c28-7dea-b506-94aba5b9bc5a_16_amount
                 */
                foreach (string key in sessionKeys) {
                    splId = Convert.ToInt32(key.Split("_")[2]);
                    if (!idAmount.ContainsKey(splId))
                        idAmount.Add(splId, Convert.ToInt16(HttpContext.Session.GetInt32($"{key}_amount")));
                }

                /*var products = from ida in idAmount
                               join p in _context.Products on ida.Key equals p.Id into pj
                               from product in pj.DefaultIfEmpty()
                               where ida.Value <= product.Amount
                               select new { product, MaxAmount = product.Amount, Amount = ida.Value };*/

                /* var products = from ida in idAmount
                                join p in _context.Products on ida.Key equals p.Id into pj
                                from product in pj.DefaultIfEmpty()
                                join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                                from photo_product_result in ppj.DefaultIfEmpty()
                                where ida.Value <= product.Amount && photo_product_result.Photo.IsMain == true
                                select new { product, MaxAmount = product.Amount, Amount = ida.Value, photo_product_result.Photo.PhotoUrl };*/

                var basket = from product in _context.Products
                               join idakey in idAmount.Keys on product.Id equals idakey into idaj
                               from iDAmount in idaj
                               join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                               from photo_product_result in ppj.DefaultIfEmpty()
                               where photo_product_result.Photo.IsMain == true
                               let discount = _context.ProductDiscounts.Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                               select new { 
                                   product, 
                                   MaxAmount = product.Amount, 
                                   Amount = idAmount[product.Id], 
                                   photo_product_result.Photo.PhotoUrl,
                                   UnitPriceName = product.PriceUnit.Name,
                                   Discount = discount.Discount.Value as decimal?,
                                   UnitDiscountName = discount.Discount.Unit.Name,
                                   DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                                   DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?
                               };

                /*var basket = from product in products
                             join photo_product in _context.PhotoProducts on product.product.Id equals photo_product.ProductId into ppj
                             from photo_product_result in ppj.DefaultIfEmpty()
                             select new { product, photo_product_result.Photo.PhotoUrl };*/
                /*var products = from product in _context.Products
                               join IDAmount in idAmount on product.Id equals IDAmount.Key into idaj
                               from IDAmount in idaj.DefaultIfEmpty()
                              *//* where IDAmount.Value <= product.Amount*//*
                               select product;*/



                /*_context.Products
                join 
                    .Where(p => idAmount.ContainsKey(p.Id))
                    .ToList()
                    .Where(p=> idAmount[p.Id] <= p.Amount)
                    .ToList(;*/

                /*foreach (var key in products)
                {
                    Console.WriteLine(key.PhotoUrl);
                }*/
            /*  List<string> sessionKeys = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
                List<int> productsId = new();
                int spl;
                foreach (var key in sessionKeys)
                {
                    spl = Convert.ToInt32(key.Split("_")[2]);
                    if (!productsId.Contains(spl))
                        productsId.Add(spl);
                }

                IQueryable<Product> products = _context.Products
                        .Where(p => productsId.Contains(p.Id))
                        .Select(p => p);

                foreach (var key in products)
                {
                    Console.WriteLine(key.Name);
                }
            */
                /*IQueryable<int> productsId = _context.Products.Select(p => p.Id);
                Dictionary<int, short> idAmount = new();
                foreach (var pId in productsId)
                {
                    int? key = HttpContext.Session.GetInt32($"product_{HttpContext.Session.GetString("newUser")}_{pId}");
                    int? value = HttpContext.Session.GetInt32($"product_{HttpContext.Session.GetString("newUser")}_{pId}_amount");
                    if (key != null && value!=null)
                        idAmount.Add(Convert.ToInt32(key), Convert.ToInt16(value));
                }
                var products = _context.Products.ToList();
                var productsUserFilter = from product in products
                                         where idAmount.ContainsKey(product.Id) && idAmount[product.Id] <= product.Amount
                                         select new { product, MaxAmount = product.Amount, Amount = idAmount[product.Id] };
*/

                /*IEnumerable<dynamic> basket = from product in productsUserFilter
                                              join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                                              from photo_product_result in ppj.DefaultIfEmpty()
                                              let discount = _context.ProductDiscounts
                                              .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                                              select new
                                              {
                                                  product,
                                                 *//* MaxAmount = product.Amount,
                                                  Amount = idAmount[product.Id],*//*
                                                  photo_product_result.Photo.PhotoUrl,
                                                  UnitPriceName = product.PriceUnit.Name,
                                                  Discount = discount.Discount.Value as decimal?,
                                                  UnitDiscountName = discount.Discount.Unit.Name,
                                                  DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                                                  DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?
                                              };
*/
                return View(basket);
                /*return View();*/

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

                string sessionAmountKey = $"product_{HttpContext.Session.GetString("newUser")}_{productRequest.ProductId}_amount";
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
