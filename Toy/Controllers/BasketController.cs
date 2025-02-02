using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using Toy.Models;
using Toy.Utilit.DataBase;
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
                                              where product.Amount > 0
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
            if (HttpContext.Session.GetString($"{GetSessionUserId()}_basket") != null)
            {
                Dictionary<int, short> idAmount = ParseSessionProducts();

                var basket = from product in _context.Products
                             where product.Amount > 0
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
                return NotFound();

            if (ProductRequestAmountIncorrect(productRequest))
                return NotFound();

            if (Request.Cookies["login"] != null)
            {
                BasketDataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null && !data.UpdateBasket(user.Id, productRequest.ProductId, Convert.ToInt16(productRequest.Amount)))
                    data.AddProductInBasket(productRequest.ProductId, user.Id, Convert.ToInt16(productRequest.Amount));
                else
                    return NotFound();
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
            Dictionary<int, short> idAmount = ParseSessionProducts();
            BasketDataBaseHelper data = new();
            User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
            if (user != null)
            {
                foreach (var key in idAmount)
                {
                    if (data.UpdateBasket(user.Id, key.Key, Convert.ToInt16(key.Value)))
                        continue;

                    data.AddProductInBasket(key.Key, user.Id, Convert.ToInt16(key.Value));
                }
            }
            else return NotFound();
            return View("_Success");
        }
        [HttpPost]
        public IActionResult Update([FromBody] ProductRequest productRequest)
        {
            if (ProductRequestIdIncorrect(productRequest))
                return NotFound();
            if (ProductRequestAmountIncorrect(productRequest))
                return NotFound();

            if (Request.Cookies["login"] != null)
            {
                BasketDataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null && data.UpdateBasket(user.Id, productRequest.ProductId, Convert.ToInt16(productRequest.Amount)))
                    return Json(new { success = true });
                else
                    return NotFound();
            }
            else
            {
                string? sessionKey = GetSessionProductAmount(productRequest.ProductId);
                if (sessionKey == null)
                    return NotFound();

                HttpContext.Session.SetInt32(sessionKey, Convert.ToInt32(productRequest.Amount));
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult Delete([FromBody] ProductRequest productRequest)
        {
            if (ProductRequestIdIncorrect(productRequest))
                return NotFound();
            if (Request.Cookies["login"] != null)
            {
                BasketDataBaseHelper data = new();
                User? user = data.GetUserByFilter(u => u.Email == Request.Cookies["login"] || u.Phone == Request.Cookies["login"]);
                if (user != null && data.DeleteBasket(user.Id, productRequest.ProductId))
                    return Json(new { success = true });
                else
                    return NotFound();
            }
            else
            {
                string? userId = GetSessionUserId();
                string? sessionKeyProduct = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}_{productRequest.ProductId}")).FirstOrDefault();
                string? sessionKeyAmount = GetSessionProductAmount(productRequest.ProductId);
                if (sessionKeyProduct == null || sessionKeyAmount == null)
                    return NotFound();

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
        private Dictionary<int, short> ParseSessionProducts()
        {
            string userId = GetSessionUserId();
            List<string> sessionKeys = HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
            Dictionary<int, short> idAmount = new();
            int splId;

            foreach (string key in sessionKeys)
            {
                splId = Convert.ToInt32(key.Split("_")[2]);
                if (!idAmount.ContainsKey(splId))
                    idAmount.Add(splId, Convert.ToInt16(HttpContext.Session.GetInt32($"{key}_amount")));
            }
            return idAmount;
        }
        private string? GetSessionUserId()
        {
            return HttpContext.Session.GetString($"newUser");
        }
        private string? GetSessionProductAmount(int productId)
        {
            return HttpContext.Session.Keys
                .Where(key => key.Contains($"product_{GetSessionUserId()}_{productId}_amount")).FirstOrDefault();
        }

    }
}
