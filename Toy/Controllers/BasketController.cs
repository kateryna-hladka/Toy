using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using Toy.Models;
using Toy.Utilit;
using Toy.Utilit.DataBase;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Toy.Controllers.BasketController;
using static Toy.Utilit.StaticVariables;

namespace Toy.Controllers
{
    public class BasketController : Controller
    {
        private readonly ToyContext _context;
        private readonly SessionHelper _sessionHelper;
        public BasketController(ToyContext context, SessionHelper sessionHelper)
        {
            _context = context;
            _sessionHelper = sessionHelper;
        }
        public IActionResult Index()
        {
            if (_del != null)
            {
                _del();
                _del = null;
            }

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
            if (HttpContext.Session.GetString($"{_sessionHelper.GetSessionUserId()}_basket") != null)
            {
                Dictionary<int, short> idAmount = _sessionHelper.ParseSessionProducts();

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
            Dictionary<int, short> idAmount = _sessionHelper.ParseSessionProducts();
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
            return View("_Info", "Успішно");
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
                string? sessionKey = _sessionHelper.GetSessionProductAmount(productRequest.ProductId);
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
                _sessionHelper.DeleteProduct(productRequest.ProductId);
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
