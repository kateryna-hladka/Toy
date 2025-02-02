using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Toy.Models;
using Toy.Utilit;
using static Toy.Utilit.Discount;
using Toy.Utilit.DataBase;
using static Toy.Controllers.BasketController;

namespace Toy.Controllers
{
    public class ProductController : Controller
    {
        private readonly ToyContext _context;
        public ProductController(ToyContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            DataBaseHelper dataBaseHelper = new();
            var product = dataBaseHelper.GetProduct(null, id);


            Dictionary<string, object> result = new();
            result.Add("product", product);
            result.Add("basketsId", dataBaseHelper.IsProductInBasket(Request, id));
            return View(result);

        }
        public IActionResult Buy()
        {
            LiqPay liqPay = new();
            var baskets = from basket in _context.Baskets
                          where basket.User.Email == Request.Cookies["login"] || basket.User.Phone == Request.Cookies["login"]
                          let discount = _context.ProductDiscounts
                          .Where(d => (basket.Product.Id == d.ProductId || basket.Product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                          select new
                          {
                              basket,
                              basket.Product.Price,
                              Discount = discount.Discount.Value as decimal?,
                              UnitDiscountName = discount.Discount.Unit.Name,
                          };
            Product[] products = [..from product in _context.Products
                                select product];

            decimal summa = 0;
            int productId;
            foreach (var b in baskets)
            {
                summa += (b.Discount != null ?
                    Utilit.Discount.GetPriceWithDiscount(b.Price, Convert.ToDecimal(b.Discount), b.UnitDiscountName)
                    : b.Price) * b.basket.Amount;
                productId = Array.FindIndex(products, p => p.Id == b.basket.ProductId);
                products[productId].Amount -= b.basket.Amount;
            }

            _context.SaveChanges();
            Dictionary<string, string> result = liqPay.CreatePayment(summa.ToString(), Request.Cookies["login"], true);
            return View(result);
        }
        
        public IActionResult Result([FromForm] string data, [FromForm] string signature)
        {
            string decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            LiqPay liqPay = new();
            string calculatedSignature = liqPay.GetLiqPaySignature(data);
            if (calculatedSignature != signature)
            {
                return View("Error", "Некоректні дані");
            }

            var paymentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedData);

            string status = paymentData["status"];
            if (status == "success" || status == "sandbox")
            {
                if (Request.Query["cookie"] == "True")
                {
                    string login = Request.Query["login"].FirstOrDefault().ToString();
                    IEnumerable<dynamic> baskets = [..from basket in _context.Baskets
                                           where basket.User.Email == login || basket.User.Phone == login
                                           select basket];
                    BasketDataBaseHelper basketData = new();
                    User? user = basketData.GetUserByFilter(u => u.Email == login || u.Phone == login);

                    string orderId;
                    string? orderQunique;
                    do
                    {
                        orderId = Guid.NewGuid().ToString();
                        orderQunique = _context.PurchaseHistoryProducts
                            .Where(p => p.PurchaseId == orderId).Select(p => p.PurchaseId).FirstOrDefault();
                    } while (orderQunique != null);

                    decimal price;
                    IEnumerable<dynamic> prod = [..from product in _context.Products
                       let discount = _context.ProductDiscounts
                       .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                       select new
                       {
                           product,
                           Discount = discount.Discount.Value as decimal?,
                           UnitDiscountName = discount.Discount.Unit.Name
                       }];

                    if (user != null)
                    {
                        foreach (var basket in baskets)
                        {
                            var z = prod.FirstOrDefault(p => p.product.Id == basket.ProductId);
                            if (z.Discount != null)
                                price = GetPriceWithDiscount(basket.Product.Price, Convert.ToDecimal(z.Discount), z.UnitDiscountName);
                            else
                                price = basket.Product.Price;

                            var purchaseHistory = new PurchaseHistory()
                            {
                                ProductId = basket.ProductId,
                                UserId = basket.UserId,
                                Date = DateOnly.FromDateTime(DateTime.Now.Date),
                                Price = price,
                                Amount = basket.Amount,
                                PaymentStatus = "Сплачено"
                            };
                            _context.PurchaseHistories.Add(purchaseHistory);
                            _context.SaveChanges();
                            _context.PurchaseHistoryProducts.Add(new PurchaseHistoryProduct() { PurchaseHistoryId = purchaseHistory.Id, PurchaseId = orderId });
                            basketData.DeleteBasket(user.Id, basket.ProductId);
                            continue;
                        }
                        _context.SaveChanges();
                    }
                    else
                        return NotFound();
                    return View("_Success", $"Номер вашого замовлення: {orderId}"); /*View(, );*/
                }
                else
                {
                    return View("_Success", $"dsdsdsd");
                }
            }
            else
            {

                return View("Error", "Оплата не здійснилась");
            }
        }
    }
}
