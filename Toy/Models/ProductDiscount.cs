using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class ProductDiscount
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? CategoryId { get; set; }

    public int DiscountId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Discount Discount { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
