using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Toy.Models;

namespace Toy.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
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
                using (ToyContext toyContext = new())
                {
                    short amount = toyContext.Products
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

            if (Request.Cookies["user-login"] != null)
            {
                using (ToyContext toyContext = new())
                {
                    toyContext.Baskets.Add(new Basket()
                    {
                        ProductId = productRequest.ProductId,
                        UserId = Convert.ToInt32(HttpContext.Session.GetInt32("userId")),
                        Amount = Convert.ToInt16(productRequest.Amount)
                    });
                    toyContext.SaveChanges();
                }
            }
            else
            {
                string sessionKey = $"product_{HttpContext.Session.GetString("newUser")}_{productRequest.ProductId}";
                HttpContext.Session.SetInt32(sessionKey, productRequest.ProductId);
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
