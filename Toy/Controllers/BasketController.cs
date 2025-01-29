using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using Toy.Models;
using Toy.Utilit;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
                                              where photo_product_result.Photo.IsMain == true
                                              let discount = _context.ProductDiscounts
                                              .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId)
                                              && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
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
                List<string> sessionKeys = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
                Dictionary<int, short> idAmount = new();
                int splId;

                foreach (string key in sessionKeys)
                {
                    splId = Convert.ToInt32(key.Split("_")[2]);
                    if (!idAmount.ContainsKey(splId))
                        idAmount.Add(splId, Convert.ToInt16(HttpContext.Session.GetInt32($"{key}_amount")));
                }

                var basket = from product in _context.Products
                             join idakey in idAmount.Keys on product.Id equals idakey into idaj
                             from iDAmount in idaj
                             join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                             from photo_product_result in ppj.DefaultIfEmpty()
                             where photo_product_result.Photo.IsMain == true
                             let discount = _context.ProductDiscounts.Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                             select new
                             {
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
                return View(basket);

            }
            return View();
        }
        [HttpPost]
        public IActionResult Add([FromBody] ProductRequest productRequest)
        {
            if (ProductRequestIdIncorrect(productRequest))
                return Json(new { success = false, message = "Invalid product ID" });

            if (ProductRequestAmountIncorrect(productRequest))
                return Json(new { success = false, message = "Invalid product amount" });

            if (Request.Cookies["login"] != null)
            {
                DataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null)
                {
                    _context.Baskets.Add(new Basket()
                    {
                        ProductId = productRequest.ProductId,
                        UserId = user.Id,
                        Amount = Convert.ToInt16(productRequest.Amount)
                    });
                    _context.SaveChanges();
                }
                else
                    return Json(new { success = false });
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
        public IActionResult AddFromSession()
        {
            Console.Write("kdkd");
            string userId = HttpContext.Session.GetString($"newUser");
            List<string> sessionKeys = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
            Dictionary<int, short> idAmount = new();
            int splId;
            foreach (string key in sessionKeys)
            {
                splId = Convert.ToInt32(key.Split("_")[2]);
                if (!idAmount.ContainsKey(splId))
                    idAmount.Add(splId, Convert.ToInt16(HttpContext.Session.GetInt32($"{key}_amount")));
            }
            DataBaseHelper data = new();
            User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
            if (user != null)
            {
                foreach (var key in idAmount)
                {
                    Basket? basket = _context.Baskets.Where(b => b.UserId == user.Id && b.ProductId == key.Key).FirstOrDefault();
                    if (basket != null)
                    {
                        basket.Amount = Convert.ToInt16(key.Value);
                        _context.SaveChanges();
                        continue;
                    }
                    _context.Baskets.Add(new Basket()
                    {
                        ProductId = key.Key,
                        UserId = user.Id,
                        Amount = Convert.ToInt16(key.Value)
                    });
                    _context.SaveChanges();
                }
            }
            else return NotFound();
            return View("_Success");
        }
        [HttpPost]
        public IActionResult Update([FromBody] ProductRequest productRequest)
        {
            if (ProductRequestIdIncorrect(productRequest))
                return Json(new { success = false, message = "Invalid product ID" });
            if (ProductRequestAmountIncorrect(productRequest))
                return Json(new { success = false, message = "Invalid product amount" });
            if (Request.Cookies["login"] != null)
            {
                DataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null)
                {
                    Basket? basket = _context.Baskets.Where(b => b.UserId == user.Id && b.ProductId == productRequest.ProductId).FirstOrDefault();
                    if (basket != null)
                    {
                        basket.Amount = Convert.ToInt16(productRequest.Amount);
                        _context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                        return Json(new { success = false });
                }
                else
                    return Json(new { success = false });
            }
            else
            {
                string userId = HttpContext.Session.GetString($"newUser");
                string sessionKey = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}_{productRequest.ProductId}_amount")).FirstOrDefault();
                if (sessionKey == null)
                    return Json(new { success = false });

                HttpContext.Session.SetInt32(sessionKey, Convert.ToInt32(productRequest.Amount));
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult Delete([FromBody] ProductRequest productRequest)
        {
            if (ProductRequestIdIncorrect(productRequest))
                return Json(new { success = false, message = "Invalid product ID" });
            if (Request.Cookies["login"] != null)
            {
                DataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null)
                {
                    Basket? basket = _context.Baskets.Where(b => b.UserId == user.Id && b.ProductId == productRequest.ProductId).FirstOrDefault();
                    if (basket != null)
                    {
                        _context.Baskets.Remove(basket);
                        _context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                        return Json(new { success = false });
                }
                else
                    return Json(new { success = false });
            }
            else
            {
                string userId = HttpContext.Session.GetString($"newUser");
                string sessionKeyProduct = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}_{productRequest.ProductId}")).FirstOrDefault();
                string sessionKeyAmount = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}_{productRequest.ProductId}_amount")).FirstOrDefault();
                if (sessionKeyProduct == null)
                    return Json(new { success = false });

                HttpContext.Session.Remove(sessionKeyProduct);
                HttpContext.Session.Remove(sessionKeyAmount);
            }
            return Json(new { success = true });
        }
        public class ProductRequest
        {
            public int ProductId { get; set; }
            public short? Amount { get; set; }
        }
        private bool ProductRequestIdIncorrect(ProductRequest? productRequest)
        {
            return (productRequest?.ProductId is not int || productRequest.ProductId <= 0);
        }
        private bool ProductRequestAmountIncorrect(ProductRequest? productRequest)
        {
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
                return true;
            return false;
        }
    }
}
