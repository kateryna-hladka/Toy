namespace Toy.Utilit
{
    public static class Discount
    {
        public static decimal GetPriceWithDiscount(decimal price, decimal discount, string unit)
        {
            return (unit) switch
            {
                "%" => price - (price * discount / 100),
                _ => price - discount

            };
        }
    }
}
