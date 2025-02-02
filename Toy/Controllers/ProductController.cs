using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Toy.Models;
using Toy.Utilit;
using static Toy.Utilit.Discount;
using Toy.Utilit.DataBase;
using static Toy.Controllers.BasketController;
using static Toy.Utilit.StaticVariables;
namespace Toy.Controllers
{
    public class ProductController : Controller
    {
        private readonly ToyContext _context;
        private readonly SessionHelper _sessionHelper;
        public ProductController(ToyContext context, SessionHelper sessionHelper)
        {
            _context = context;
            _sessionHelper = sessionHelper;
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
        [HttpPost]
        public IActionResult Buy()
        {
            LiqPay liqPay = new();
            Product[] products = GetProducts();
            decimal summa = 0;
            decimal getSumma = 0;

            Dictionary<string, string> result = null;

            if (Request.Cookies["login"] != null)
            {
                string login = Request.Cookies["login"];
                var baskets = GetBusketWithDiscount(login);

                foreach (var basket in baskets)
                {
                    getSumma = SubtractProductAmountAndGetPrice(basket, basket.Price, basket.basket.Amount, products, basket.basket.ProductId);
                    if (getSumma < 0)
                        return RedirectToAction("Index", "Basket");
                    summa += getSumma;
                }

                _context.SaveChanges();
                result = liqPay.CreatePayment(summa.ToString(), login, true);
            }
            else
            {
                var baskets = GetBasketFromSession();

                foreach (var basket in baskets)
                {
                    getSumma = SubtractProductAmountAndGetPrice(basket, basket.product.Price, basket.Amount, products, basket.product.Id);
                    if (getSumma < 0)
                        return RedirectToAction("Index", "Basket");
                    summa += getSumma;
                }

                _context.SaveChanges();
                result = liqPay.CreatePayment(summa.ToString(), _sessionHelper.GetSessionUserId(), false);
            }

            return View(result);
        }

        public IActionResult Result([FromForm] string data, [FromForm] string signature)
        {
            string decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            LiqPay liqPay = new();
            string calculatedSignature = liqPay.GetLiqPaySignature(data);
            if (calculatedSignature != signature)
            {
                return View("_Info", "Некоректні дані");
            }

            var paymentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedData);

            string status = paymentData["status"];
            if (status == "success" || status == "sandbox")
            {
                string login = Request.Query["login"].FirstOrDefault().ToString();
                string orderId = GenerateUniqueOrderId();
                string[] viewParametr = [$"Номер вашого замовлення: {orderId}", "clearStorage"];

                if (Request.Query["cookie"] == "True")
                {
                    IEnumerable<dynamic> baskets = [..from basket in _context.Baskets
                                                      where basket.User.Email == login || basket.User.Phone == login
                                                      select basket];
                    BasketDataBaseHelper basketData = new();
                    User? user = basketData.GetUserByFilter(u => u.Email == login || u.Phone == login);

                    decimal price;
                    IEnumerable<dynamic> products = [..from product in _context.Products
                                                    let discount = _context.ProductDiscounts
                                                    .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) 
                                                    && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
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
                            var product = products.FirstOrDefault(p => p.product.Id == basket.ProductId);
                            if (product.Discount != null)
                                price = GetPriceWithDiscount(basket.Product.Price, Convert.ToDecimal(product.Discount), product.UnitDiscountName);
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
                        }
                        _context.SaveChanges();
                    }
                    else
                        return NotFound();
                    return View("_Info", viewParametr);
                }
                else
                {
                    _del = _sessionHelper.DeleteAllProducts;
                    return View("_Info", viewParametr);
                }
            }
            else
            {
                string login = Request.Query["login"].FirstOrDefault().ToString();
                
                if (Request.Query["cookie"] == "True")
                    AddProductAmount(GetBusketWithDiscount(login));
                else
                    AddProductAmount(GetBasketFromSession());
                
                return View("_Info", "Оплата не здійснена");
            }
        }
        private string GenerateUniqueOrderId()
        {
            string orderId;
            string? orderQunique;
            do
            {
                orderId = Guid.NewGuid().ToString();
                orderQunique = _context.PurchaseHistoryProducts
                    .Where(p => p.PurchaseId == orderId).Select(p => p.PurchaseId).FirstOrDefault();
            } while (orderQunique != null);
            return orderId;
        }

        private Product[] GetProducts()
        {
            return [.. from product in _context.Products select product];
        }

        private dynamic GetBusketWithDiscount(string login)
        {
            return from basket in _context.Baskets
                   where basket.User.Email == login || basket.User.Phone == login
                   let discount = _context.ProductDiscounts
                   .Where(d => (basket.Product.Id == d.ProductId || basket.Product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                   select new
                   {
                       basket,
                       basket.Product.Price,
                       Discount = discount.Discount.Value as decimal?,
                       UnitDiscountName = discount.Discount.Unit.Name,
                   };
        }

        private decimal SubtractProductAmountAndGetPrice(dynamic baskets, decimal price, short amount, Product[] products, int findIndexId)
        {
            decimal summa = 0;
            int productId;
            summa += (baskets.Discount != null ?
                Utilit.Discount.GetPriceWithDiscount(price, Convert.ToDecimal(baskets.Discount), baskets.UnitDiscountName)
                : price) * amount;
            productId = Array.FindIndex(products, p => p.Id == findIndexId);
            if (products[productId].Amount >= amount)
                products[productId].Amount -= amount;
            else
            {
                if (Request.Cookies["login"] != null)
                {
                    string login = Request.Cookies["login"];
                    BasketDataBaseHelper data = new();
                    data.DeleteBasket(data.GetUserByFilter(u => u.Email == login || u.Phone == login).Id, products[productId].Id);
                    return -1;
                }
                else
                {
                    _sessionHelper.DeleteProduct(products[productId].Id);
                    return -1;
                }
            }

            return summa;
        }

        private dynamic GetBasketFromSession()
        {
            Dictionary<int, short> idAmount = _sessionHelper.ParseSessionProducts();

            return from product in _context.Products
                   join idakey in idAmount.Keys on product.Id equals idakey into idaj
                   from iDAmount in idaj
                   let discount = _context.ProductDiscounts
                  .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                   select new
                   {
                       product,
                       MaxAmount = product.Amount,
                       Amount = idAmount[product.Id],
                       Discount = discount.Discount.Value as decimal?,
                       UnitDiscountName = discount.Discount.Unit.Name
                   };
        }

        private void AddProductAmount(dynamic baskets)
        {
            Product[] products = GetProducts();
            int productId;
            foreach (var b in baskets)
            {
                productId = Array.FindIndex(products, p => p.Id == b.product.Id);
                products[productId].Amount += b.Amount;
            }
            _context.SaveChanges();
        }
    }
}
