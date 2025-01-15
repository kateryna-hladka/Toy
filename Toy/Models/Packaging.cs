using System;
using System.Collections.Generic;

namespace Toy.Models;

public partial class Packaging
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Length { get; set; }

    public decimal? Width { get; set; }

    public decimal? Hight { get; set; }

    public int? UnitId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Unit? Unit { get; set; }
}
