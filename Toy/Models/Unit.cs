using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Unit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Packaging> Packagings { get; set; } = new List<Packaging>();

    public virtual ICollection<Product> ProductSizeUnits { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductWeightUnits { get; set; } = new List<Product>();
}
