using System.Web;
using Toy.Models;
using static Toy.Controllers.BasketController;
namespace Toy.Utilit
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<int, short> ParseSessionProducts()
        {
            List<string> sessionKeys = GetProductsId();
            Dictionary<int, short> idAmount = new();
            int splId;

            foreach (string key in sessionKeys)
            {
                splId = Convert.ToInt32(key.Split("_")[2]);
                if (!idAmount.ContainsKey(splId))
                    idAmount.Add(splId, Convert.ToInt16(_httpContextAccessor.HttpContext.Session.GetInt32($"{key}_amount")));
            }
            return idAmount;
        }
        public string? GetSessionUserId()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString($"newUser");
        }

        public List<string> GetProductsId()
        {
            string userId = GetSessionUserId();
            return _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
        }

        public string? GetSessionProductAmount(int productId)
        {
            return _httpContextAccessor.HttpContext?.Session.Keys
                .Where(key => key.Contains($"product_{GetSessionUserId()}_{productId}_amount")).FirstOrDefault();
        }
        public void DeleteAllProducts()
        {
            List<string> productsId = GetProductsId();
            List<string> amount = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{GetSessionUserId()}") && key.EndsWith("_amount")).ToList();
            foreach (var i in productsId)
                RemoveSessionItem(i);
            foreach(var i in amount)
                RemoveSessionItem(i);
        }
        public void DeleteProduct(int productId)
        {
            string? sessionKeyProduct = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{GetSessionUserId()}_{productId}")).FirstOrDefault();
            string? sessionKeyAmount = GetSessionProductAmount(productId);

            RemoveSessionItem(sessionKeyProduct);
            RemoveSessionItem(sessionKeyAmount);
        }
        public void RemoveSessionItem(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
