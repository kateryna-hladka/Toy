using Toy.Models;

namespace Toy.Utilit
{
    public static class NewProducts
    {
        private static readonly int countNewProduct = 10;
        public static int[] GetProducts()
        {
            using (ToyContext toyContext = new())
            {
                return [.. toyContext.Products
            .OrderByDescending(p => p.Id)
            .Take(countNewProduct)
            .Select(s => s.Id)];
            }
        }
    }
}
