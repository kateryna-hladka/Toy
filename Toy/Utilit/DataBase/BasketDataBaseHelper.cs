using Microsoft.EntityFrameworkCore;
using Toy.Models;

namespace Toy.Utilit.DataBase
{
    public class BasketDataBaseHelper : DataBaseHelper
    {
        ToyContext _context { get; set; }
        public BasketDataBaseHelper()
        {
            _context = new ToyContext();
        }
        public void AddProductInBasket(int productId, int userId, short amount)
        {
            _context.Baskets.Add(new Basket()
            {
                ProductId = productId,
                UserId = userId,
                Amount = amount
            });
            _context.SaveChanges();
        }
        public Basket? GetBasket(int userId, int productId)
        => _context.Baskets.Where(b => b.UserId == userId && b.ProductId == productId).FirstOrDefault();

        public bool UpdateBasket(int userId, int productId, short amount)
        {
            Basket? basket = GetBasket(userId, productId);
            if (basket != null)
            {
                basket.Amount = amount;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteBasket(int userId, int productId)
        {
            Basket? basket = GetBasket(userId, productId);
            if (basket != null)
            {
                _context.Baskets.Remove(basket);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
