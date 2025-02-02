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
            string userId = GetSessionUserId();
            List<string> sessionKeys = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
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
        public string? GetSessionProductAmount(int productId)
        {
            return _httpContextAccessor.HttpContext?.Session.Keys
                .Where(key => key.Contains($"product_{GetSessionUserId()}_{productId}_amount")).FirstOrDefault();
        }
        public void DeleteAllProducts()
        {
            string userId = GetSessionUserId();
            List<string> productsId = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && !key.EndsWith("_amount")).ToList();
            List<string> amount = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}") && key.EndsWith("_amount")).ToList();
            foreach (var i in productsId)
                _httpContextAccessor.HttpContext.Session.Remove(i);
            foreach(var i in amount)
                _httpContextAccessor.HttpContext.Session.Remove(i);
        }
        public void DeleteProduct(int productId)
        {
            string? userId = GetSessionUserId();
            string? sessionKeyProduct = _httpContextAccessor.HttpContext.Session.Keys.Where(key => key.Contains($"product_{userId}_{productId}")).FirstOrDefault();
            string? sessionKeyAmount = GetSessionProductAmount(productId);

            _httpContextAccessor.HttpContext.Session.Remove(sessionKeyProduct);
            _httpContextAccessor.HttpContext.Session.Remove(sessionKeyAmount);
        }
    }
}
