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
            if ((productRequest?.ProductId is not int || productRequest.ProductId <= 0) ||
                (productRequest.Amount != null && productRequest.Amount <= 0))
            {
                return Json(new { success = false, message = "Invalid product ID" });
            }
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
