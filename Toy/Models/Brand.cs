using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PhotoBrand> PhotoBrands { get; set; } = new List<PhotoBrand>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
