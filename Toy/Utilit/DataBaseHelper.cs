using System.Linq.Expressions;
using Toy.Models;

namespace Toy.Utilit
{
    public class DataBaseHelper
    {
        ToyContext ToyContext { get; set; }
        public DataBaseHelper()
        {
            ToyContext = new ToyContext();
        }
        public dynamic GetProduct(int? CategoryId, int? productId)
        {
            var products = from product in ToyContext.Products
                           join photo_product in ToyContext.PhotoProducts on product.Id equals photo_product.ProductId into ppj
                           from photo_product_result in ppj.DefaultIfEmpty()
                           let discount = ToyContext.ProductDiscounts
                           .Where(d => (product.Id == d.ProductId || product.CategoryId == d.CategoryId) && DateTime.Now <= d.Discount.DateTimeEnd).FirstOrDefault()
                           let mark = ToyContext.Reviews.Where(r => r.ProductId == product.Id).Average(r => r.Mark)
                           let comment = ToyContext.Reviews.Where(r => r.ProductId == product.Id).Count()
                           where (CategoryId != null && (product.CategoryId == CategoryId && (photo_product_result.Photo.IsMain == true || photo_product_result == null)))
                           || (productId !=null && product.Id == productId)
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
            return (productId != null) ? products.FirstOrDefault() : products;
        }

    }
}
