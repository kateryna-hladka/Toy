using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
