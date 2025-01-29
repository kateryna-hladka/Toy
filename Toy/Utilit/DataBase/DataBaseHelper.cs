using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Toy.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Toy.Controllers.BasketController;

namespace Toy.Utilit.DataBase
{
    public class DataBaseHelper
    {
        ToyContext _context { get; set; }
        public DataBaseHelper()
        {
            _context = new ToyContext();
        }
        public dynamic? GetProduct(int? CategoryId, int? productId)
        {
            var products = from product in _context.Products
                           join photo_product in _context.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                           from photo_product_result in ppj.DefaultIfEmpty()
                           let discount = _context.ProductDiscounts
                           .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                           let mark = _context.Reviews.Where(r => r.ProductId == product.Id).Average(r => r.Mark)
                           let comment = _context.Reviews.Where(r => r.ProductId == product.Id).Count()
                           where CategoryId != null && product.CategoryId == CategoryId && (photo_product_result.Photo.IsMain == true || photo_product_result == null)
                           || productId != null && product.Id == productId
                           select new
                           {
                               product,
                               product.Brand,
                               product.CountryProducer,
                               product.Material,
                               product.Packaging,
                               product.Category,
                               UnitWeightName = product.WeightUnit.Name,
                               UnitSizeName = product.SizeUnit.Name,
                               UnitPriceName = product.PriceUnit.Name,
                               photo_product_result.Photo.PhotoUrl,
                               Discount = discount.Discount.Value as decimal?,
                               UnitDiscountName = discount.Discount.Unit.Name,
                               DateTimeStart = discount.Discount.DateTimeStart as DateTime?,
                               DateTimeEnd = discount.Discount.DateTimeEnd as DateTime?,
                               MarkAvg = mark as double?,
                               CommentCount = comment as int?
                           };
            return productId != null ? products.FirstOrDefault() : products;
        }

        public User? GetUserByFilter(Func<User, bool> filter)
        => _context.User
                    .Where(filter)
                    .Select(u => new User() { Id = u.Id, Name = u.Name, Surname = u.Surname, Email = u.Email, Phone = u.Phone, Password = u.Password })
                    .FirstOrDefault();
    }
}
